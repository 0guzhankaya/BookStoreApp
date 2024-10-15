using AutoMapper;
using BookStore.Entities.DataTransferObjects;
using BookStore.Entities.Exceptions;
using BookStore.Entities.Models;
using BookStore.Entities.RequestFeatures;
using BookStore.Repositories.Contracts;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager repositoryManager, ILoggerService loggerService, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _loggerService = loggerService;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _repositoryManager.Book.CreateOneBook(entity);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookAndCheckExists(id, trackChanges);

            _repositoryManager.Book.DeleteOneBook(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            if (!bookParameters.ValidPriceRange) 
                throw new PriceOutOfRangeBadRequestException();

            var booksWithMetaData = await _repositoryManager.Book.GetAllBooksAsync(bookParameters, trackChanges);
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData); 
            return (booksDto, booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBookAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookAndCheckExists(id, trackChanges);
            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            // check entity
            var entity = await GetOneBookAndCheckExists(id, trackChanges);

            // Mapping
            entity = _mapper.Map<Book>(bookDto);

            _repositoryManager.Book.Update(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookAndCheckExists(id, trackChanges);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _repositoryManager.SaveAsync();
        }

        private async Task<Book> GetOneBookAndCheckExists(int id, bool trackChanges)
        {
            // check entity
            var entity = await _repositoryManager.Book.GetOneBookAsync(id, trackChanges);

            if (entity is null) throw new BookNotFoundException(id);

            return entity;
        }
    }
}

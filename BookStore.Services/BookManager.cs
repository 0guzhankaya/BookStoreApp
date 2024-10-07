using AutoMapper;
using BookStore.Entities.DataTransferObjects;
using BookStore.Entities.Exceptions;
using BookStore.Entities.Models;
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

        public Book CreateOneBook(Book book)
        {
            _repositoryManager.Book.CreateOneBook(book);
            _repositoryManager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            // check entity
            var entity = _repositoryManager.Book.GetOneBook(id, trackChanges);

            if (entity is null) throw new BookNotFoundException(id);

            _repositoryManager.Book.DeleteOneBook(entity);
            _repositoryManager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _repositoryManager.Book.GetAllBooks(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books); // Book --> BookDto
        }

        public Book GetOneBook(int id, bool trackChanges)
        {
            var book = _repositoryManager.Book.GetOneBook(id, trackChanges);
            if (book is null) throw new BookNotFoundException(id);
            return book;
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            // check entity
            var entity = _repositoryManager.Book.GetOneBook(id, trackChanges);
            if (entity is null) throw new BookNotFoundException(id);

            // Mapping
            entity = _mapper.Map<Book>(bookDto);

            _repositoryManager.Book.Update(entity);
            _repositoryManager.Save();
        }
    }
}

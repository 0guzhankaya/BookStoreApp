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

        public BookManager(IRepositoryManager repositoryManager, ILoggerService loggerService)
        {
            _repositoryManager = repositoryManager;
            _loggerService = loggerService;
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

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _repositoryManager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBook(int id, bool trackChanges)
        {
            var book = _repositoryManager.Book.GetOneBook(id, trackChanges);
            if (book is null) throw new BookNotFoundException(id);
            return book;
        }

        public void UpdateOneBook(int id, Book book, bool trackChanges)
        {
            // check entity
            var entity = _repositoryManager.Book.GetOneBook(id, trackChanges);
            if (entity is null) throw new BookNotFoundException(id);

            // check params
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            entity.Title = book.Title;
            entity.Price = book.Price;

            _repositoryManager.Book.Update(entity);
            _repositoryManager.Save();
        }
    }
}

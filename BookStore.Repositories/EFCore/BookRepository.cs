using BookStore.Entities.Models;
using BookStore.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges).OrderBy(b => b.Id);

        public IQueryable<Book> GetOneBook(int id, bool trackChanges) => FindByCondition(b => b.Id.Equals(id), trackChanges);

        public void UpdateOneBook(Book book) => Update(book);
    }
}

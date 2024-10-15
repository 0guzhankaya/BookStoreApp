using BookStore.Entities.Models;
using BookStore.Entities.RequestFeatures;
using BookStore.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            var books = await FindAll(trackChanges).FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
                .OrderBy(b => b.Id).ToListAsync();
            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<Book> GetOneBookAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}

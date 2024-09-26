using BookStore.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repositories.EFCore
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext _repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void Create(T entity) => _repositoryContext.Set<T>().Add(entity);

        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _repositoryContext.Set<T>().AsNoTracking() : _repositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> experssion, bool trackChanges) =>
            !trackChanges ? _repositoryContext.Set<T>().Where(experssion).AsNoTracking()
            :
            _repositoryContext.Set<T>().Where(experssion);

        public void Update(T entity) => _repositoryContext.Set<T>().Update(entity);
    }
}

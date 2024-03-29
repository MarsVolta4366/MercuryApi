﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MercuryApi.Data.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task Create(T entity);
        void Delete(T entity);
        IQueryable<T> RunSqlRaw(string sql);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected MercuryDbContext Context { get; set; }

        public RepositoryBase(MercuryDbContext context)
        {
            Context = context;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            trackChanges ?
            Context.Set<T>() : Context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            trackChanges ?
            Context.Set<T>().Where(expression) : Context.Set<T>().Where(expression).AsNoTracking();

        public async Task Create(T entity) =>
            await Context.Set<T>().AddAsync(entity);

        public void Delete(T entity) => Context.Set<T>().Remove(entity);

        public IQueryable<T> RunSqlRaw(string sql) =>
            Context.Set<T>().FromSqlRaw(sql);
    }
}

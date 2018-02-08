
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherData.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly DbContext Context; 

        public Repository(DbContext context)
        {
            Context = context; 
        }

        public int Count() { 
			return Context.Set<TEntity>().Count(); 
		}

		public async Task<int> CountAsync() 
		{ 
			return await Context.Set<TEntity>().CountAsync(); 
		}

		public TEntity Get(int id)
		{   
			return Context.Set<TEntity>().Find(id);
		}

		public TEntity Get(string id) 
		{
			return Context.Set<TEntity>().Find(id);
		}

		public async Task<TEntity> GetAsync(int id) 
		{ 
			return await Context.Set<TEntity>().FindAsync(id);
		}

		public async Task<TEntity> GetAsync(string id) 
		{
			return await Context.Set<TEntity>().FindAsync(id); 
		}

		public IEnumerable<TEntity> GetAll()
		{
			return Context.Set<TEntity>().ToList();
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await Context.Set<TEntity>().ToListAsync();
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			return Context.Set<TEntity>().Where(predicate);
		}

		public void Add(TEntity entity)
		{
			Context.Set<TEntity>().Add(entity);
		}

		public async Task AddAsync(TEntity entity)
		{
			await Context.Set<TEntity>().AddAsync(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>().AddRange(entities);
		}

		public async Task AddRangeAsync(IEnumerable<TEntity> entities)
		{
			await Context.Set<TEntity>().AddRangeAsync(entities);
		}

		public void Remove(TEntity entity)
		{
			Context.Set<TEntity>().Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>().RemoveRange(entities);
		}
    }
}
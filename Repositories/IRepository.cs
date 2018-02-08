
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WeatherData.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count(); 
        Task<int> CountAsync(); 
        TEntity Get(int id);
		TEntity Get(string id); 
		Task<TEntity> GetAsync(int id);
		Task<TEntity> GetAsync(string id); 
		IEnumerable<TEntity> GetAll();
		Task<IEnumerable<TEntity>> GetAllAsync();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
		void Add(TEntity entity);
		Task AddAsync(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);
		Task AddRangeAsync(IEnumerable<TEntity> entities);
		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
    }
}
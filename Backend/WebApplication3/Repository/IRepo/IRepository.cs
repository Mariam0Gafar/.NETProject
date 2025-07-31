using System.Linq.Expressions;

namespace WebApplication3.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> sortBy = null, bool descending = false);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}

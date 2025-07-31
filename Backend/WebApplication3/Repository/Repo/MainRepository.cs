using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.Repo
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
        internal AppDbContext dbContext;
        internal readonly DbSet<T> dbSet;

        public MainRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> sortBy = null, bool descending = false)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (sortBy != null)
                query = descending ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

            return await query.Select(selector).ToListAsync();
        }


        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);

            }
                return await query.FirstOrDefaultAsync();        
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }
    }
}

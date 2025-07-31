using WebApplication3.Data;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class DepRepository : MainRepository<Department>, IDepRepository
    {

        public DepRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Department> GetDepByName(string dep)
        {
            return dbSet.Where(d => d.Name == dep).FirstOrDefault();
        }

        public IQueryable<Department> GetAllQueryable()
        {
            return dbContext.Set<Department>().AsQueryable();
        }
    }
}

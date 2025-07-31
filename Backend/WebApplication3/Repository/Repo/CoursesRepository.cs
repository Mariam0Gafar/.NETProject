using System.Runtime.Intrinsics.Arm;
using WebApplication3.Data;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.Repo
{
    public class CoursesRepository : MainRepository<Course>, ICoursesRepository
    {

        public CoursesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Course> GetCourseByName(string c)
        {
            return dbSet.Where(d => d.Name == c).FirstOrDefault();
        }

        public IQueryable<Course> GetAllQueryable()
        {
            return dbContext.Set<Course>().AsQueryable();
        }
    }
}

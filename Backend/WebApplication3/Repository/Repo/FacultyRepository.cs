using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class FacultyRepository : MainRepository<Faculty>, IFacultyRepository
    {

    public FacultyRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Faculty> GetFacultyByName(string dep)
        {
            return dbSet.Where(d => d.Name == dep).FirstOrDefault();
        }

        public async Task<IEnumerable<FacultyViewModel>> GetFacultyWithDepAndCourses(int id)
        {
            return await dbSet.Where(f => f.Id == id)
               .Include(f => f.Departments)
               .Select(f => new FacultyViewModel()
               {
                   isActive = f.isActive,
                   facultyName = f.Name,
                   departments = f.Departments.Select(ff => new DepartmentsWithCoursesViewModel()
                   {
                       depName = ff.Name,
                       courses = ff.Courses.Select(c => new courseViewModel()
                       {
                           Name = c.Name,
                           Description = c.Description
                       }).ToList()
                   }).ToList()                  
               })
               .ToListAsync();
        }
        public IQueryable<Faculty> GetAllQueryable()
        {
            return dbContext.Set<Faculty>().AsQueryable();
        }
    }
}

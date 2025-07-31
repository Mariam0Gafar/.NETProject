using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class StudentRepository : MainRepository<Student>, IStudentRepository
    {

        public StudentRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<studentWithCourseViewModel>> GetStudentCoursesWithDepId(int depId)
        {
            return await dbSet.Where(s => s.DepartmentId == depId)
                .Include(s => s.Courses)
                .Select(s => new studentWithCourseViewModel()
                {
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    DepartmentId = s.DepartmentId,
                    Courses = s.Courses.Select(sc => new courseViewModel()
                    {
                        Name = sc.Course.Name,
                        Description = sc.Course.Description
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<studentViewModel>> GetStudentsWithDepId(int depId)
        {
            return await dbSet
                    .Where(s => s.DepartmentId == depId)
                    .Select(s => new studentViewModel
                    {
                        Name = s.Name,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber,
                        DepartmentId = s.DepartmentId
                    })
                    .ToListAsync();
        }
        public IQueryable<Student> GetAllQueryable()
        {
            if (dbContext == null)
            {
                throw new Exception("dbContext is null in StudentRepository");
            }

            return dbContext.Set<Student>().AsQueryable();
        }

    }
}

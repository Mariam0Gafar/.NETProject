using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;
using WebApplication3.Services.Service;

namespace WebApplication3.Repository.Repo
{
    public class StudentCourseRepository : MainRepository<StudentCourse>, IStudentCourseRepository
    {
        private readonly AppDbContext dbContext;

        public StudentCourseRepository(AppDbContext dbContext) : base(dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<CoursesWithDep> GetCoursesByStudentId(int studentId)
        {           
            var student = await dbContext.Set<Student>()
            .Include(s => s.Department)
            .Include(s => s.Courses)
            .ThenInclude(s => s.Course)
            .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return null;

            return new CoursesWithDep
            {
                Department = new departmentViewModel
                {
                    Name = student.Department.Name,
                    Description = student.Department.Description
                },
                Courses = student.Courses.Select(sc => new courseViewModel
                {
                    Name = sc.Course?.Name,
                    Description = sc.Course?.Description
                }).ToList()
            };

        }
    }

    
}

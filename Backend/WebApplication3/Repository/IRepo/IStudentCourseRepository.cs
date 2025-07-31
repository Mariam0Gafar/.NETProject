using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Repository.Repo;
using WebApplication3.Services.Service;

namespace WebApplication3.Repository.IRepo
{
    public interface IStudentCourseRepository : IRepository<StudentCourse>
    {
        Task<CoursesWithDep> GetCoursesByStudentId(int studentId);
    }
}

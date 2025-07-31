using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.IRepo
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<studentViewModel>> GetStudentsWithDepId(int depId);
        Task<IEnumerable<studentWithCourseViewModel>> GetStudentCoursesWithDepId(int depId);
        
    }
}

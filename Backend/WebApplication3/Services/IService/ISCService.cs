using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface ISCService
    {
        Task<ServiceResult<bool>> Create(studentCourseBindingModel sc);

        Task<ServiceResult<IEnumerable<StudentCourse>>> GetAll();

        Task<ServiceResult<StudentCourse>> GetById(int scId);

        Task<ServiceResult<bool>> Update(studentCourseBindingModel sc);

        Task<ServiceResult<bool>> Delete(int scId);
        Task<ServiceResult<CoursesWithDep>> GetCoursesByStudentId(int studentId);
    }
}

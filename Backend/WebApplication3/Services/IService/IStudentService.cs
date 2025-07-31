using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface IStudentService
    {
        Task<ServiceResult<bool>> Create(studentBindingModel student);

        Task<ServiceResult<IEnumerable<Student>>> GetAll();

        Task<ServiceResult<studentViewModel>> GetById(int studentId);

        Task<ServiceResult<bool>> Update(studentBindingModel student);

        Task<ServiceResult<bool>> Delete(int studentId);
        Task<ServiceResult<IEnumerable<studentViewModel>>> GetStudentsByDepId(int depId);
        Task<ServiceResult<IEnumerable<studentWithCourseViewModel>>> GetStudentCoursesWithDepId(int depId);
        Task<ServiceResult<bool>> TakeExam(int studentId, int examId);
        Task<bool> SubmitExam(int examId);
        Task<PagedList<studentViewModel>> GetPaginatedStudents(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
    }
}

using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface ICourseService
    {
        Task<ServiceResult<bool>> CreateCourse(courseBindingModel Course);

        Task<IEnumerable<courseViewModel>> GetAllCourses(Expression<Func<Course, bool>> filter = null, Expression<Func<Course, object>> sortBy = null, bool descending = false);
        Task<courseViewModel> GetCourseById(int courseId);
        Task<ServiceResult<bool>> UpdateCourse(courseBindingModel Course);

        Task<ServiceResult<bool>> DeleteCourse(int courseId);
        Task<Course> GetCourseByName(string c);

        Task<PagedList<courseViewModel>> GetPaginatedCourses(
           string? name,
           string? sortBy,
           string? sortOrder,
           int page,
           int pageSize,
           CancellationToken cancellationToken);

    }
}

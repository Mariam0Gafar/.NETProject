using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface IFacultyService
    {
        Task<IEnumerable<FacultyViewModel>> GetFacultyWithDepAndCourses(int id);
        Task<ServiceResult<IEnumerable<Faculty>>> GetAll();

        Task<Faculty> GetByName(string name);

        Task<ServiceResult<bool>> Create(FacultyBindingModel f);
        Task<ServiceResult<Faculty>> GetById(int fId);

        Task<ServiceResult<bool>> Update(FacultyBindingModel f);

        Task<ServiceResult<bool>> Delete(int fId);

        Task<PagedList<FacultyViewModel>> GetPaginatedFaculties(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

    }
}

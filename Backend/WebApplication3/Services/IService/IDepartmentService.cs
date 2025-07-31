using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface IDepartmentService
    {
        Task<ServiceResult<bool>> Create(departmentBindingModel dep);

        Task<ServiceResult<IEnumerable<Department>>> GetAll();

        Task<ServiceResult<Department>> GetById(int depId);

        Task<ServiceResult<bool>> Update(departmentBindingModel dep);

        Task<ServiceResult<bool>> Delete(int depId);

        Task<Department> GetByName(string name);

        Task<PagedList<departmentViewModel>> GetPaginatedDepartments(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

    }
}

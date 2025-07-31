using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<bool>> Create(departmentBindingModel dep)
        {
            if (dep == null)
                return ServiceResult<bool>.Fail("Department data is null");

            var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync(dep.FacultyId);
            if(faculty == null)
            {
                return ServiceResult<bool>.Fail("Faculty not found");
            }
            if (faculty.isActive == false)
            {
                return ServiceResult<bool>.Fail("Faculty is Inactive");
            }

            Department c = new Department()
            {
                Name = dep.Name,
                Description = dep.Description,
                FacultyId = dep.FacultyId  
            };
            await _unitOfWork.DepartmentRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create Department");

        }

        public async Task<ServiceResult<bool>> Delete(int depId)
        {
            if (depId <= 0)
                return ServiceResult<bool>.Fail("Invalid department ID");

            var depDetails = await _unitOfWork.DepartmentRepository.GetByIdAsync(depId);
            if (depDetails == null)
                return ServiceResult<bool>.Fail("Department not found");

            _unitOfWork.DepartmentRepository.Delete(depId);
             var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to Delete Department");

        }

        public async Task<ServiceResult<IEnumerable<Department>>> GetAll()
        {
            var depDetailsList = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Department>>.Ok(depDetailsList);
        }

        public async Task<ServiceResult<Department>> GetById(int depId)
        {
            if (depId <= 0)
                return ServiceResult<Department>.Fail("Invalid department ID");

            var depDetails = await _unitOfWork.DepartmentRepository.GetByIdAsync(depId);
            if (depDetails == null)
                return ServiceResult<Department>.Fail("Department not found");
            return ServiceResult<Department>.Ok(depDetails);

        }

        public async Task<Department> GetByName(string name)
        {
            var dep = await _unitOfWork.DepartmentRepository.GetDepByName(name);
            return dep;
        }

        public async Task<ServiceResult<bool>> Update(departmentBindingModel dep)
        {
            if (dep == null)
                return ServiceResult<bool>.Fail("Department data is null");


            var depDetails = await _unitOfWork.DepartmentRepository.GetByIdAsync(dep.Id);
            if (depDetails == null)
                return ServiceResult<bool>.Fail("Department not found");

            depDetails.Name = dep.Name;
            depDetails.Description = dep.Description;
                   
            _unitOfWork.DepartmentRepository.Update(depDetails);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to Update Department");
        }

        public async Task<PagedList<departmentViewModel>> GetPaginatedDepartments(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            Expression<Func<Department, bool>> filter = string.IsNullOrWhiteSpace(name) ? null : c => c.Name.Contains(name);

            Expression<Func<Department, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => c => c.Name,
                "description" => c => c.Description,
                "id" => c => c.Id,
                _ => c => c.Name
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            IQueryable<Department> deps = _unitOfWork.DepartmentRepository.GetAllQueryable();

            if (filter != null)
                deps = deps.Where(filter);

            deps = isDescending ? deps.OrderByDescending(orderBy) : deps.OrderBy(orderBy);

            var response = deps.Select(c => new departmentViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                FacultyId = c.FacultyId,
                
            });

            return await PagedList<departmentViewModel>.CreateAsync(response, page, pageSize, cancellationToken);
        }
    }
    
}

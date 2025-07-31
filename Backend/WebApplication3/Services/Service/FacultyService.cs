using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;

namespace WebApplication3.Services.Service
{
    public class FacultyService : IFacultyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacultyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FacultyViewModel>> GetFacultyWithDepAndCourses(int id)
        {
            return await _unitOfWork.FacultyRepository.GetFacultyWithDepAndCourses(id);
        }

        public async Task<ServiceResult<IEnumerable<Faculty>>> GetAll()
        {
            var faculties = await _unitOfWork.FacultyRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Faculty>>.Ok(faculties);
        }

        public async Task<Faculty> GetByName(string name)
        {
            var f = await _unitOfWork.FacultyRepository.GetFacultyByName(name);
            return f;
        }

        public async Task<ServiceResult<bool>> Create(FacultyBindingModel f)
        {
            if (f == null)
                return ServiceResult<bool>.Fail("Faculty data is null");

           
            Faculty c = new Faculty()
            {
                Name = f.facultyName,
                isActive = f.isActive,
            };
            await _unitOfWork.FacultyRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create Faculty");
        }

        public async Task<ServiceResult<Faculty>> GetById(int fId)
        {
            if (fId <= 0)
                return ServiceResult<Faculty>.Fail("Invalid faculty ID");

            var fDetails = await _unitOfWork.FacultyRepository.GetByIdAsync(fId);
            if (fDetails == null)
                return ServiceResult<Faculty>.Fail("faculty not found");
            return ServiceResult<Faculty>.Ok(fDetails);
        }

        public async Task<ServiceResult<bool>> Update(FacultyBindingModel f)
        {
            if (f == null)
                return ServiceResult<bool>.Fail("Faculty data is null");


            var fDetails = await _unitOfWork.FacultyRepository.GetByIdAsync(f.id);
            if (fDetails == null)
                return ServiceResult<bool>.Fail("Faculty not found");

            fDetails.Name = f.facultyName;
            fDetails.isActive = f.isActive;

            _unitOfWork.FacultyRepository.Update(fDetails);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to Update Faculty");
        
    }

        public async Task<ServiceResult<bool>> Delete(int fId)
        {
            if (fId <= 0)
                return ServiceResult<bool>.Fail("Invalid faculty ID");

            var fDetails = await _unitOfWork.FacultyRepository.GetByIdAsync(fId);
            if (fDetails == null)
                return ServiceResult<bool>.Fail("Faculty not found");

            _unitOfWork.FacultyRepository.Delete(fId);
            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to Delete Faculty");
        }

        public async Task<PagedList<FacultyViewModel>> GetPaginatedFaculties(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            Expression<Func<Faculty, bool>> filter = string.IsNullOrWhiteSpace(name) ? null : c => c.Name.Contains(name);

            Expression<Func<Faculty, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => c => c.Name,
                "id" => c => c.Id,
                _ => c => c.Name  
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            IQueryable<Faculty> f = _unitOfWork.FacultyRepository.GetAllQueryable();

            if (filter != null)
                f = f.Where(filter);

            f = isDescending ? f.OrderByDescending(orderBy) : f.OrderBy(orderBy);

            var response = f.Select(c => new FacultyViewModel
            {
                Id = c.Id,
                facultyName = c.Name,
                isActive = c.isActive
            });

            return await PagedList<FacultyViewModel>.CreateAsync(response, page, pageSize, cancellationToken);
        }
    }
}

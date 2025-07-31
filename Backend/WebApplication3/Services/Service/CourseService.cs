using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Repository.Repo;
using WebApplication3.Services.IService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication3.Services.Service
{
    public class CourseService : ICourseService
    {
        private IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<bool>> CreateCourse(courseBindingModel Course)
        {
            if (Course == null)
                return ServiceResult<bool>.Fail("Course data is null");

            if (Course.DepartmentId != null)
            {
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(Course.DepartmentId.Value);
                if (department == null)
                    return ServiceResult<bool>.Fail("The department ID specified is not valid!");
            }

            var c = new Course
            {
                Name = Course.Name,
                Description = Course.Description,
                DepartmentId = Course.DepartmentId
            };

            await _unitOfWork.CourseRepository.AddAsync(c);

            var result = _unitOfWork.SaveChanges();

            return result > 0
                ? ServiceResult<bool>.Ok(true)
                : ServiceResult<bool>.Fail("Failed to create course");
        }


        public async Task<ServiceResult<bool>> DeleteCourse(int courseId)
        {
            if (courseId <= 0)
                return ServiceResult<bool>.Fail("Invalid course ID");
            var courseDetails = await _unitOfWork.CourseRepository.GetByIdAsync(filter: c => c.Id == courseId);
            if (courseDetails == null)
                return ServiceResult<bool>.Fail("Course data is null");

            _unitOfWork.CourseRepository.Delete(courseId);
            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to create course");

        }

        public async Task<IEnumerable<courseViewModel>> GetAllCourses(Expression<Func<Course, bool>> filter = null, Expression<Func<Course, object>> sortBy = null, bool descending = false)
        {
            Expression<Func<Course, courseViewModel>> selector = x => new courseViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            };
            return await _unitOfWork.CourseRepository.GetAllAsync(selector: selector, filter: filter, sortBy: sortBy, descending: descending); 
        }


        public async Task<courseViewModel> GetCourseById(int courseId)
        {
           
            var result = await _unitOfWork.CourseRepository.GetByIdAsync(filter: c => c.Id == courseId);
            if(result != null)
            {
                return new courseViewModel()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description
                };
            }
            else
            {
                return null;
            }
          
        }

        public async Task<Course> GetCourseByName(string c)
        {
            var f = await _unitOfWork.CourseRepository.GetCourseByName(c);
            return f;
        }

        public async Task<ServiceResult<bool>> UpdateCourse(courseBindingModel Course)
        {
            if (Course == null)
                return ServiceResult<bool>.Fail("Course data is null");

            var courseDetails = await _unitOfWork.CourseRepository.GetByIdAsync(filter: c => c.Id == Course.Id);
            if (courseDetails == null)
                return ServiceResult<bool>.Fail("Course not found");

            if (Course.DepartmentId != null)
            {
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(Course.DepartmentId.Value);
                if (department == null)
                    return ServiceResult<bool>.Fail("The department ID specified is not valid!");
                courseDetails.DepartmentId = Course.DepartmentId;
            }
            else
            {
                courseDetails.DepartmentId = courseDetails.DepartmentId;
            }

            courseDetails.Name = Course.Name;
            courseDetails.Description = Course.Description;

            _unitOfWork.CourseRepository.Update(courseDetails);

            var result = _unitOfWork.SaveChanges();
            return result > 0 ? ServiceResult<bool>.Ok(true) : ServiceResult<bool>.Fail("Failed to update course");

        }

        public async Task<PagedList<courseViewModel>> GetPaginatedCourses(
            string? name,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            Expression<Func<Course, bool>> filter = string.IsNullOrWhiteSpace(name)? null : c => c.Name.Contains(name);

            Expression<Func<Course, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => c => c.Name,
                "description" => c => c.Description,
                "id" => c => c.Id,
                _ => c => c.Name
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            IQueryable<Course> courses = _unitOfWork.CourseRepository.GetAllQueryable();

            if (filter != null)
                courses = courses.Where(filter);

            courses = isDescending ? courses.OrderByDescending(orderBy) : courses.OrderBy(orderBy);

            var courseResponse = courses.Select(c => new courseViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DepartmentId = c.DepartmentId
            });

            return await PagedList<courseViewModel>.CreateAsync(courseResponse, page, pageSize, cancellationToken);
        }

    }
}

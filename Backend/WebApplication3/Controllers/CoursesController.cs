using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await courseService.GetCourseById(id);
            if(result == null)
            {
                return NotFound("This course is not found");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses(string? name, string? sortBy, string? sortOrder = "asc")
        {
            Expression<Func<Course, bool>> filter = string.IsNullOrWhiteSpace(name) ? null : x => x.Name == name;
            Expression<Func<Course, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => x => x.Name,
                "description" => x => x.Description,
                "id" => x => x.Id,
                _ => x => x.Id
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            var result = await courseService.GetAllCourses(filter, orderBy, isDescending);

           

            var coursesPerPage = result
                .ToList();

            return Ok(new
            {
                courses = coursesPerPage,
            });
        }



        [HttpPost]
        public async Task<IActionResult> AddCourse(courseBindingModel course)
        {
            var result = await courseService.CreateCourse(course);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(courseBindingModel course)
        {
            var result = await courseService.UpdateCourse(course);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await courseService.DeleteCourse(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("name")]
        public async Task<IActionResult> GetCourseByName([FromQuery] string name)
        {
            var result = await courseService.GetCourseByName(name);
            if (result == null)
            {
                return NotFound("This Course is not found");
            }
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCourses(
            string? name,
            string? sortBy,
            string? sortOrder = "asc",
            int page = 1,
            int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            var result = await courseService.GetPaginatedCourses(name, sortBy, sortOrder, page, pageSize, cancellationToken);

            return Ok(new
            {
                courses = result.Items,
                page = result.Page,
                totalCount = result.TotalCount,
                totalPages = (int)Math.Ceiling((double)result.TotalCount / result.PageSize),
                hasNext = result.HasNextPage,
                hasPrevious = result.HasPreviousPage
            });
        }


    }
}

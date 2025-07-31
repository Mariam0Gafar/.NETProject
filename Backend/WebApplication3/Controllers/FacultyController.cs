using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            this.facultyService = facultyService;
        }

        [HttpGet]
        [Route("dep/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await facultyService.GetFacultyWithDepAndCourses(id);
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllFaculties()
        {
            var result = await facultyService.GetAll();
            return result.Success ? Ok(result.Data.Select(c => new FacultyViewModel() { Id=c.Id, facultyName = c.Name, isActive = c.isActive })) : BadRequest(result.Message);
        }


        [HttpGet]
        [Route("name")]
        public async Task<IActionResult> GetFacultyByName([FromQuery] string name)
        {
            var result = await facultyService.GetByName(name);
            if (result == null)
            {
                return NotFound("This faculty is not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetFacultyById(int id)
        {
            var result = await facultyService.GetById(id);
            return result.Success ? Ok(new FacultyViewModel() { facultyName = result.Data.Name, isActive = result.Data.isActive }) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFaculty(FacultyBindingModel f)
        {
            var result = await facultyService.Create(f);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFaculty(FacultyBindingModel f)
        {
            var result = await facultyService.Update(f);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            var result = await facultyService.Delete(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedFaculties(
          string? name,
          string? sortBy,
          string? sortOrder = "asc",
          int page = 1,
          int pageSize = 5,
          CancellationToken cancellationToken = default)
        {
            var result = await facultyService.GetPaginatedFaculties(name, sortBy, sortOrder, page, pageSize, cancellationToken);

            return Ok(new
            {
                faculties = result.Items,
                page = result.Page,
                totalCount = result.TotalCount,
                totalPages = (int)Math.Ceiling((double)result.TotalCount / result.PageSize),
                hasNext = result.HasNextPage,
                hasPrevious = result.HasPreviousPage
            });
        }
    }
}

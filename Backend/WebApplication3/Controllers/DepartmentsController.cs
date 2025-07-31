using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await departmentService.GetAll();
            return result.Success ? Ok(result.Data.Select(c => new departmentViewModel() { Id = c.Id, Name = c.Name, Description = c.Description })) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await departmentService.GetById(id);
            return result.Success ? Ok(new departmentViewModel() { Id = result.Data.Id, Name = result.Data.Name, Description = result.Data.Description }) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(departmentBindingModel department)
        {
            var result = await departmentService.Create(department);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(departmentBindingModel department)
        {
            var result = await departmentService.Update(department);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await departmentService.Delete(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("name")]
        public async Task<IActionResult> GetDepByName([FromQuery] string name)
        {
            var result = await departmentService.GetByName(name);
            if (result == null)
            {
                return NotFound("This department is not found");
            }
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedDeps(
           string? name,
           string? sortBy,
           string? sortOrder = "asc",
           int page = 1,
           int pageSize = 5,
           CancellationToken cancellationToken = default)
        {
            var result = await departmentService.GetPaginatedDepartments(name, sortBy, sortOrder, page, pageSize, cancellationToken);

            return Ok(new
            {
                departments = result.Items,
                page = result.Page,
                totalCount = result.TotalCount,
                totalPages = (int)Math.Ceiling((double)result.TotalCount / result.PageSize),
                hasNext = result.HasNextPage,
                hasPrevious = result.HasPreviousPage
            });
        }

    }
}

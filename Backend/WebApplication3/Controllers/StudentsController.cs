using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await studentService.GetAll();           
            return result.Success? Ok(result.Data.Select(c => new studentViewModel() {Id= c.Id, Name = c.Name, Email = c.Email, DepartmentId = c.DepartmentId, PhoneNumber = c.PhoneNumber })) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await studentService.GetById(id);
            return result.Success ? Ok(new studentViewModel() { Name = result.Data.Name, Email = result.Data.Email, DepartmentId = result.Data.DepartmentId, PhoneNumber = result.Data.PhoneNumber }) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(studentBindingModel student)
        {
            var result = await studentService.Create(student);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);

        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(studentBindingModel student)
        {
            var result = await studentService.Update(student);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await studentService.Delete(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("Department/{id}")]
        public async Task<IActionResult> GetStudentsWithDepId(int id)
        {
            var result = await studentService.GetStudentsByDepId(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("DepartmentWithCourses/{id}")]
        public async Task<IActionResult> GetStudentCoursesWithDepId(int id)
        {
            var result = await studentService.GetStudentCoursesWithDepId(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedStudents(
          string? name,
          string? sortBy,
          string? sortOrder = "asc",
          int page = 1,
          int pageSize = 5,
          CancellationToken cancellationToken = default)
        {
            var result = await studentService.GetPaginatedStudents(name, sortBy, sortOrder, page, pageSize, cancellationToken);

            return Ok(new
            {
                students = result.Items,
                page = result.Page,
                totalCount = result.TotalCount,
                totalPages = (int)Math.Ceiling((double)result.TotalCount / result.PageSize),
                hasNext = result.HasNextPage,
                hasPrevious = result.HasPreviousPage
            });
        }

    }
}

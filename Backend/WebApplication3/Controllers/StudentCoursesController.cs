using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.IService;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentCoursesController : ControllerBase
    {

        private readonly ISCService studentCourseService;

        public StudentCoursesController(ISCService studentCourseService)
        {
            this.studentCourseService = studentCourseService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudentCourseById(int id)
        {
            var result = await studentCourseService.GetById(id);
            return result.Success ? Ok(new studentCourseViewModel() { courseId = result.Data.courseId, studentId = result.Data.studentId, Grade = result.Data.Grade }) : BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentCourses()
        {
            var result = await studentCourseService.GetAll();
            return result.Success ? Ok(result.Data.Select(c => new studentCourseViewModel() { courseId = c.courseId, studentId = c.studentId, Grade = c.Grade })) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentCourse(studentCourseBindingModel course)
        {
            var result = await studentCourseService.Create(course);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudentCourse(studentCourseBindingModel course)
        {
            var result = await studentCourseService.Update(course);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudentCourse(int id)
        {
            var result = await studentCourseService.Delete(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("CoursesForStudent")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            var result = await studentCourseService.GetCoursesByStudentId(studentId);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

    }
}

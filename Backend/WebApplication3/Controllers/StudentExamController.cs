using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentExamController : ControllerBase
    {
        private readonly ISXService sxService;

        public StudentExamController(ISXService sxService)
        {
            this.sxService = sxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentExams(bool? isSubmitted, string? sortBy, string? sortOrder = "asc", int page = 1, int pageSize = 5)
        { //orderBY , SearchKey , Expression


            Expression<Func<StudentExam, bool>> filter = isSubmitted == null ? null : x => x.isSubmitted == isSubmitted;
            Expression<Func<StudentExam, object>> orderBy = sortBy?.ToLower() switch
            {
                "id" => x => x.Id,
                "studentid" => x => x.StudentId,
                "examid" => x => x.ExamId,
                "grade" => x => x.Grade,
                _ => null
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            var result = await sxService.GetAllStudentExams(filter, orderBy, isDescending);

            var totalCount = result.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var examsPerPage = result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(examsPerPage);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllStudentExams(bool? isSubmitted, string? sortBy, string? sortOrder = "asc")
        { //orderBY , SearchKey , Expression


            Expression<Func<StudentExam, bool>> filter = isSubmitted == null ? null : x => x.isSubmitted == isSubmitted;
            Expression<Func<StudentExam, object>> orderBy = sortBy?.ToLower() switch
            {
                "id" => x => x.Id,
                "studentid" => x => x.StudentId,
                "examid" => x => x.ExamId,
                "grade" => x => x.Grade,
                _ => null
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            var result = await sxService.GetAllStudentExams(filter, orderBy, isDescending);
            var examsPerPage = result
                .ToList();

            return Ok(examsPerPage);
        }

        [HttpPost]
        [Route("TakeExam")]
        public async Task<IActionResult> AddStudentExam(int studentId, int examId)
        {
            var result = await sxService.TakeExam(studentId, examId);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        [Route("SubmitExam")]
        public async Task<IActionResult> SubmitExam(int sxId)
        {
            var result = await sxService.SubmitExam(sxId);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        [Route("AddGrade")]
        public async Task<IActionResult> AddGrade(int sxId, int grade)
        {
            var result = await sxService.AddGrade(sxId, grade);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("GetGradesForStudent")]
        public async Task<IActionResult> GetGradesforStudent(int studentId)
        {
            var result = await sxService.GetGradesForStudent(studentId);

            return Ok(result);
        }

        [HttpGet]
        [Route("prepareSubmission")]
        public async Task<IActionResult> PrepareSubmission(int examId, int studentId)
        {
            var model = await sxService.PrepareSubmissionAsync(examId, studentId);
            if (model == null)
                return NotFound("Exam not found");

            return Ok(model);
        }
   

        [HttpPost]
        [Route("Submit")]
        public async Task<IActionResult> Submit(SubmitExamBindingModel x)
        {
            if (!x.isActive)
            {
                return BadRequest("Exam is not active!");
            }
            var sx = await sxService.GetExamWithStudent(x.ExamId);
            foreach (var s in sx)
            {
                if (s.StudentId == x.StudentId)
                {
                    return BadRequest("student already took exam");
                }
            }
            
            var (grade, correct, total) = await sxService.Submit(x);

            return Ok(new
            {
                grade = grade,
                correct = correct,
                total = total,
            });
        }

        [HttpPost]
        [Route("Grades")]
        public async Task<IActionResult> GradesForExam(int examId)
        {

            var result = await sxService.GetGradesForExam(examId);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService examService;

        public ExamController(IExamService examService)
        {
            this.examService = examService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetExamById(int id)
        {
            var result = await examService.GetExamById(id);
            if (result == null)
            {
                return NotFound("This exam is not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/Exam{id}")]
        public async Task<IActionResult> GetExam(int id)
        {
            var result = await examService.GetExam(id);
            if (result == null)
            {
                return NotFound("This exam is not found");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExams(int? examId, string? sortBy, string? sortOrder = "asc", int page = 1, int pageSize = 5)
        { //orderBY , SearchKey , Expression

            
            Expression<Func<Exam, bool>> filter = examId == null? null : x => x.Id == examId;
            Expression<Func<Exam, object>> orderBy = sortBy?.ToLower() switch
            {
                "id" => x => x.Id,
                _ => null
            };

            bool isDescending = sortOrder?.ToLower() == "desc" || sortOrder?.ToLower() == "descending";

            var result = await examService.GetAllExams(filter, orderBy, isDescending);

            var totalCount = result.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var examsPerPage = result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(examsPerPage);
        }


        [HttpPost]
        public async Task<IActionResult> AddExam(ExamBindingModel exam)
        {
            var result = await examService.Create(exam);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Unable to create exam");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExam(ExamBindingModel exam)
        {
            var result = await examService.Update(exam);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var result = await examService.Delete(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        [Route("MakeActive")]
        public async Task<IActionResult> MakeActive(int id)
        {
            var result = await examService.Active(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedExams(
          string? name,
          string? sortBy,
          string? sortOrder = "asc",
          int page = 1,
          int pageSize = 5,
          CancellationToken cancellationToken = default)
        {
            var result = await examService.GetPaginatedExams(name, sortBy, sortOrder, page, pageSize, cancellationToken);

            return Ok(new
            {
                exams = result.Items,
                page = result.Page,
                totalCount = result.TotalCount,
                totalPages = (int)Math.Ceiling((double)result.TotalCount / result.PageSize),
                hasNext = result.HasNextPage,
                hasPrevious = result.HasPreviousPage
            });
        }

    }
}

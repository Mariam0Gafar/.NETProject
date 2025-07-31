using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services.IService;
using WebApplication3.Services.Service;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;

        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(QuestionBindingModel q)
        {
            var result = await questionService.Create(q);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(QuestionBindingModel q)
        {
            var result = await questionService.UpdateQuestion(q);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        [Route("Exam")]
        public async Task<IActionResult> GetQuestionsByExamId(int id)
        {
            var result = await questionService.GetQuestionsByExamId(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}

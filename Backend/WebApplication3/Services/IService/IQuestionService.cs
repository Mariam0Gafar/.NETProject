using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface IQuestionService
    {
        Task<ServiceResult<bool>> Create(QuestionBindingModel q);
        Task<ServiceResult<bool>> UpdateQuestion(QuestionBindingModel q);
        Task<ServiceResult<IEnumerable<QuestionBindingModel>>> GetQuestionsByExamId(int examId);

        Task<ServiceResult<Question>> GetById(int qId);


    }
}

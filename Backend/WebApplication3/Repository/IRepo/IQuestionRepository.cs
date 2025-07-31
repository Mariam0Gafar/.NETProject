using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.IRepo
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionBindingModel>> GetQuestionsByExamId(int examId);

    }
}

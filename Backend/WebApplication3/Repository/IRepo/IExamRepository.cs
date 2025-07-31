using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.IRepo
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<ExamViewModel> GetExamWithQuestions(int examId);

    }
}

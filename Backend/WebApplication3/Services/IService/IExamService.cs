using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface IExamService
    {
        Task<Exam> Create(ExamBindingModel exam);

        Task<IEnumerable<ExamViewModel>> GetAllExams(Expression<Func<Exam, bool>> filter = null, Expression<Func<Exam, object>> sortBy = null, bool descending = false);

        Task<ExamViewModel> GetExamById(int examId);
        Task<ExamBindingModel> GetExam(int examid);
        

        Task<ServiceResult<bool>> Update(ExamBindingModel exam);

        Task<ServiceResult<bool>> Delete(int examId);
        Task<ServiceResult<bool>> Active(int examId);

        Task<PagedList<ExamViewModel>> GetPaginatedExams(
                    string? name,
                    string? sortBy,
                    string? sortOrder,
                    int page,
                    int pageSize,
                    CancellationToken cancellationToken);

    }
}

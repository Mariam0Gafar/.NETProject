using System.Linq.Expressions;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services.Service;

namespace WebApplication3.Services.IService
{
    public interface ISXService
    {
        Task<IEnumerable<StudentExamViewModel>> GetAllStudentExams(Expression<Func<StudentExam, bool>> filter = null, Expression<Func<StudentExam, object>> sortBy = null, bool descending = false);
        Task<ServiceResult<bool>> TakeExam(int studentId, int examId);
        Task<SubmitExamBindingModel> PrepareSubmissionAsync(int examId, int studentId);
        Task<ServiceResult<bool>> SubmitExam(int sxId);
        Task<(double grade,int correct, int total)> Submit(SubmitExamBindingModel x);
        Task<ServiceResult<bool>> AddGrade(int sxId, int grade);

        Task<IEnumerable<StudentExamViewModel>> GetGradesForStudent(int studentId);
        Task<IEnumerable<GradesForExamViewModel>> GetGradesForExam(int examId);
        Task<IEnumerable<GradesForExamViewModel>> GetExamWithStudent(int examId);



    }
}

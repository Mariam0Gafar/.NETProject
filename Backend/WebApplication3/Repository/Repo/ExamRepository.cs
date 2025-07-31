using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class ExamRepository : MainRepository<Exam>, IExamRepository
    {

        public ExamRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ExamViewModel> GetExamWithQuestions(int examId)
        {
            return await dbSet
                        .Where(e => e.Id == examId)
                        .Include(e => e.Questions)
                        .Select(e => new ExamViewModel
                        {
                            Id = e.Id,
                            Title = e.Title,
                            CourseId = e.CourseId,
                            Questions = e.Questions.Select(q => new QuestionViewModel()
                            {
                                Id = q.Id,
                                Question = q.Description
                            }).ToList(),
                            isActive = e.isActive
                        })
                        .FirstOrDefaultAsync();
        }
        public async Task<Exam> GetExamWithStudents(int id)
        {
            return await dbSet
                        .Where(e => e.Id == id)
                        .Include(e => e.Students)                   
                        .FirstOrDefaultAsync();
        }

        public IQueryable<Exam> GetAllQueryable()
        {
            return dbContext.Set<Exam>().AsQueryable();
        }
    }
}

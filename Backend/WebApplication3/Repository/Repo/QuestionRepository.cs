using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class QuestionRepository : MainRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext dbContext;

        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionBindingModel>> GetQuestionsByExamId(int examId)
        {
            return await dbSet
                        .Where(e => e.ExamId == examId)
                        .Select(e => new QuestionBindingModel
                        {
                            Id = e.Id,
                            Description = e.Description,
                            CorrectAnswer = e.CorrectAnswer,    
                            ExamId = e.ExamId
                        })
                        .ToListAsync();
        }
    }
    }

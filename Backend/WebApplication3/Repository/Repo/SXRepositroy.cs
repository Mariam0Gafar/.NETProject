using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.IRepo;

namespace WebApplication3.Repository.Repo
{
    public class SXRepositroy : MainRepository<StudentExam>, ISXRepository
    {
        private readonly AppDbContext dbContext;

    public SXRepositroy(AppDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<GradesForExamViewModel>> GetExamWithStudent(int examId)
        {
            return await dbSet
                        .Where(e => e.ExamId == examId && e.Grade != null)
                        .Include(e => e.Student)
                        .Select(e => new GradesForExamViewModel
                        {
                            StudentId = e.StudentId,
                            Grade = e.Grade,
                            Name = e.Student.Name
                        }).ToListAsync();
        }
    }
}

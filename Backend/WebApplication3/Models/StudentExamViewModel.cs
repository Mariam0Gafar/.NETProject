using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class StudentExamViewModel
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public double? Grade { get; set; }
        public bool isSubmitted { get; set; }
    }
}

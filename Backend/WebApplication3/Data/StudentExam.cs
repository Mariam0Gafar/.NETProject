using Microsoft.Identity.Client;

namespace WebApplication3.Data
{
    public class StudentExam
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public double? Grade { get; set; }
        public bool isSubmitted { get; set; }
        public ICollection<StudentAnswer> studentAnswers { get; set; }

    }
}

namespace WebApplication3.Data
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public string studentAnswer { get; set; }
        public int questionId { get; set; }
        public Question Question { get; set; }
        public int studentExamId { get; set; }
        public StudentExam StudentExam { get; set; }
    }
}

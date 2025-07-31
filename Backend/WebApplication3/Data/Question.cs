namespace WebApplication3.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CorrectAnswer { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}

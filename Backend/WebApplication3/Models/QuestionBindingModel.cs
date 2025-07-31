using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class QuestionBindingModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CorrectAnswer { get; set; }
        public int ExamId { get; set; }
    }
}

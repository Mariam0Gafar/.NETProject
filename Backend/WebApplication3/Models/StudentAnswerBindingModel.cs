using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class StudentAnswerBindingModel
    {
        public int Id { get; set; }
        public string answer { get; set; }
        public int questionId { get; set; }
        public int studentExamId { get; set; }
    }
}

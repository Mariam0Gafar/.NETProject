using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class SubmitExamBindingModel
    {
        public string Title { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public bool isActive { get; set; }
        public List<StudentAnswerViewModel> studentAnswers { get; set; }
    }
}

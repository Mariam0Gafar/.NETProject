using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class ExamBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool isActive { get; set; }
    }
}

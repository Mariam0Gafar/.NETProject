namespace WebApplication3.Models
{
    public class studentCourseBindingModel
    {
        public int Id { get; set; }

        public int studentId { get; set; }
        public int courseId { get; set; }
        public int? Grade { get; set; }
    }
}

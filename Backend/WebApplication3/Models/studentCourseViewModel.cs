using System.Text.Json.Serialization;
using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class studentCourseViewModel
    {
        public int studentId { get; set; }
        public int courseId { get; set; }
        public int? Grade { get; set; }
    }
}

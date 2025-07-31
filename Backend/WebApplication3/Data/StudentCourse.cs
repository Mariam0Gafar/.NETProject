using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication3.Data
{
    public class StudentCourse
    {
        public int Id { get; set; }
        public int studentId { get; set; }

        public Student? Student { get; set; }
        public int courseId { get; set; }

        public Course? Course { get; set; }
        public int? Grade { get; set; }
    }
}

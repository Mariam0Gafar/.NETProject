using System.Text.Json.Serialization;

namespace WebApplication3.Data
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<StudentCourse>? Students { get; set; }
        

    }
}

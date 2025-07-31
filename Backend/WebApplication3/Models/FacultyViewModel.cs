namespace WebApplication3.Models
{
    public class FacultyViewModel
    {
        public int Id { get; set; }
        public string facultyName { get; set; }
        public bool isActive { get; set; }
        public List<DepartmentsWithCoursesViewModel> departments { get; set; }
    }
}

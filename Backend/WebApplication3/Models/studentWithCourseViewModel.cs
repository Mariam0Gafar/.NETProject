namespace WebApplication3.Models
{
    public class studentWithCourseViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public List<courseViewModel> Courses { get; set; }
    }
}

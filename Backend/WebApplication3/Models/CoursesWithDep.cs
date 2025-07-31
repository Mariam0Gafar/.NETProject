using WebApplication3.Data;

namespace WebApplication3.Models
{
    public class CoursesWithDep
    {
        public List<courseViewModel> Courses { get; set; }
        public departmentViewModel Department { get; set; }

    }
}

using WebApplication3.Data;

namespace WebApplication3.Repository.Base
{
    public interface ICoursesRepository : IRepository<Course>
    {
        Task<Course> GetCourseByName(string c);

    }
}

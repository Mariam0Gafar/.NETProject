using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.IRepo
{
    public interface IFacultyRepository : IRepository<Faculty>
    {
        Task<IEnumerable<FacultyViewModel>> GetFacultyWithDepAndCourses(int Id);

        Task<Faculty> GetFacultyByName(string dep);

    }
}

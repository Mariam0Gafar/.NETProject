using WebApplication3.Data;
using WebApplication3.Repository.Repo;

namespace WebApplication3.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        CoursesRepository CourseRepository { get; }
        StudentRepository StudentRepository { get; }
        DepRepository DepartmentRepository { get; }
        StudentCourseRepository SCRepository { get; }
        ExamRepository ExamRepository { get; }
        SXRepositroy SXRepositroy { get; }
        QuestionRepository QuestionRepository { get; }
        FacultyRepository FacultyRepository { get; }




        int SaveChanges();
    }
}

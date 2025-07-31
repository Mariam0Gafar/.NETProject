using WebApplication3.Data;
using WebApplication3.Repository.Base;

namespace WebApplication3.Repository.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private CoursesRepository courseRepository;
        private StudentRepository studentRepository;
        private DepRepository departmentRepository;
        private StudentCourseRepository scRepository;
        private ExamRepository examRepository;
        private SXRepositroy sxRepository;
        private QuestionRepository questionRepository;
        private FacultyRepository facultyRepository;


        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CoursesRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new CoursesRepository(_dbContext);
                }
                return courseRepository;
            }
        }

        public ExamRepository ExamRepository
        {
            get
            {
                if (this.examRepository == null)
                {
                    this.examRepository = new ExamRepository(_dbContext);
                }
                return examRepository;
            }
        }

        public FacultyRepository FacultyRepository
        {
            get
            {
                if (this.facultyRepository == null)
                {
                    this.facultyRepository = new FacultyRepository(_dbContext);
                }
                return facultyRepository;
            }
        }

        public QuestionRepository QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new QuestionRepository(_dbContext);
                }
                return questionRepository;
            }
        }

        public SXRepositroy SXRepositroy
        {
            get
            {
                if (this.sxRepository == null)
                {
                    this.sxRepository = new SXRepositroy(_dbContext);
                }
                return sxRepository;
            }
        }

        public StudentRepository StudentRepository
        {
            get
            {
                if (this.studentRepository == null)
                {
                    this.studentRepository = new StudentRepository(_dbContext);
                }
                return studentRepository;
            }
        }

        public DepRepository DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new DepRepository(_dbContext);
                }
                return departmentRepository;
            }
        }

        public StudentCourseRepository SCRepository
        {
            get
            {
                if (this.scRepository == null)
                {
                    this.scRepository = new StudentCourseRepository(_dbContext);
                }
                return scRepository;
            }
        }


        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

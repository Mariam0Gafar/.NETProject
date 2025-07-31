using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        DbSet<Student> Students { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<StudentCourse> StudentCourses { get; set; }
        DbSet<Exam> Exam { get; set; }
        DbSet<StudentExam> StudentExam { get; set; }
        DbSet<StudentAnswer> StudentAnswer { get; set; }
        DbSet<Question> Question { get; set; }
        DbSet<Faculty> Faculties { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>()
                .HasKey(s => s.Id);        
            builder.Entity<Student>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Department>()
                .HasKey(d => d.Id);
            builder.Entity<Department>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Course>()
                .HasKey(c => c.Id);
            builder.Entity<Course>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Exam>()
                .HasKey(e => e.Id);
            builder.Entity<Exam>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<StudentExam>()
                .HasKey(se => se.Id);
            builder.Entity<StudentExam>()
                .Property(se => se.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<StudentAnswer>()
                .HasKey(sa => sa.Id);
            builder.Entity<StudentAnswer>()
                .Property(sa => sa.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Question>()
                .HasKey(q => q.Id);
            builder.Entity<Question>()
                .Property(q => q.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Faculty>()
                .HasKey(f => f.Id);
            builder.Entity<Faculty>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<StudentCourse>()
                .HasKey(sc => sc.Id);
            builder.Entity<StudentCourse>()
                .Property(sc => sc.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Student>()
                .HasOne(s => s.IdentityUser)
                .WithMany()
                .HasForeignKey(s => s.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication3.Data
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }
        public ICollection<StudentCourse>? Courses { get; set; }
        public ICollection<StudentExam>? Exams { get; set; }

    }
}

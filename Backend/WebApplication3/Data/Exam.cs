namespace WebApplication3.Data
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId {get; set;}
        public ICollection<Question> Questions { get; set; }
        public Course Course { get; set; }
        public ICollection<StudentExam>? Students { get; set; }
        public bool isActive { get; set; }

    }
}

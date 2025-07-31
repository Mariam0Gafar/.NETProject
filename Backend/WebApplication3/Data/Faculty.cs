namespace WebApplication3.Data
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}

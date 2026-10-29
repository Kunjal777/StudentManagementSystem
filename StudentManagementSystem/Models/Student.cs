namespace StudentManagementSystem.Models
{
    public class Student
    {
        public string? Name { get; set; }
        public int Roll_No { get; set; }
        public int DOB {  get; set; }
        public string? Gender { get; set; }
        public int Id { get; set; }
        public string TeacherUsername { get; set; } = string.Empty;
    }
}

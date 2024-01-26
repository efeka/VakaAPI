namespace VakaAPI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string TC { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public int EmployeeType { get; set; }
    }
}

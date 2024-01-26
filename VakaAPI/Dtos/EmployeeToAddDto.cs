namespace VakaAPI.Models
{
    public class EmployeeToAddDto
    {
        public string TC { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public int EmployeeType { get; set; }
    }
}

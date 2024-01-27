namespace VakaAPI.Models
{
    public class Attendence
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalDaysWorked { get; set; }
        public int ExtraHoursWorked { get; set; }
    }
}

namespace Presensi360.Models
{
    public class AttendanceModel
    {
        public int EmployeeId { get; set; }
        public int Day { get; set; }
        public DateTime? Date { get; set; }
        public string? Code { get; set; }
        public decimal? CheckIn { get; set; }
        public decimal? CheckOut { get; set; }
        public decimal? Late { get; set; }
        public string? Note { get; set; }
        public int? MealAllowance { get; set; }
    }
}

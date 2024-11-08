using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Employee
    {
        private readonly string _TABLE = "employee";
        [Key]
        public string? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public int? UserId { get; set; }

        //Relational Model
        public Users? User { get; set; } //Belongs to Users

        //Log Attributes
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

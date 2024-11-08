using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Department
    {
        private readonly string _TABLE = "department";

        [Key]
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }

        //Relational Model
        public ICollection<SubDepartment>? SubDepartments { get; set; } //Has many SubDepartments

        //attributes log
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

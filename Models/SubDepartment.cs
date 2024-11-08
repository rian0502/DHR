using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class SubDepartment
    {
        private readonly string _TABLE = "sub_department";
        [Key]
        public int SubDepartmentID { get; set; }
        public string? SubDepartmentName { get; set; }
        public string? SubDepartmentCode { get; set; }
        public Guid? DepartmentID { get; set; }

        //Relational Model
        public Department? Department { get; set; } // Belongs to Departement

        //attributes log
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class SubDepartmentModel
    {
        [Key]
        public int SubDepartmentID { get; set; }
        public string? SubDepartmentName { get; set; }
        public string? SubDepartmentCode { get; set; }
        public int? DepartmentID { get; set; }

        //Relational Model
        public DepartmentModel? Department { get; set; } // Belongs to Department
        public ICollection<DivisionModel>? Divisions { get; set; } //Has many Divisions

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

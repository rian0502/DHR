using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class SubDepartmentModel
    {
        [Key]
        public int SubDepartmentId { get; set; }
        public string? SubDepartmentName { get; set; }
        public string? SubDepartmentCode { get; set; }
        public int DepartmentId { get; set; }

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

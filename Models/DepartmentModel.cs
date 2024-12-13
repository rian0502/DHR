using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public int? CompanyId { get; set; }

        //Relational Model
        public CompanyModel? Company { get; set; } // Belongs to Company
        public ICollection<SubDepartmentModel>? SubDepartments { get; set; } //Has many SubDepartments

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

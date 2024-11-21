using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class DivisionModel
    {
        [Key]
        public int DivisionId { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public int SubDepartmentId { get; set; }

        //Relationship Models
        public SubDepartmentModel SubDepartment { get; set; }
        public ICollection<EmployeeModel> Employees { get; set; }
        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

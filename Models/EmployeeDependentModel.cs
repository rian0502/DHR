using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class EmployeeDependentModel
    {
        [Key]
        public int EmployeeDependentId { get; set; }

        public string? DependentName { get; set; }
        public string? DependentGender { get; set; }
        public string? DependentStatus { get; set; }
        public int? EmployeeId { get; set; }

        //Relational Model
        public EmployeeModel? Employee { get; set; } // Belongs to Employee

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

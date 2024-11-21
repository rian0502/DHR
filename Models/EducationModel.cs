using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class EducationModel
    {
        [Key]
        public int EducationId { get; set; }
        public string? EducationName { get; set; }

        //Relational Model
        public ICollection<EmployeeModel>? Employees { get; set; } // Has many Employee

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

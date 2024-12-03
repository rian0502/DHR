using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class JobTitleModel
    {
        [Key]   
        public int JobTitleId { get; set; }
        public string? JobTitleName { get; set; }
        public string? JobTitleCode { get; set; }
        public string? JobTitleDescription { get; set; }

        //Relational Model
        public ICollection<EmployeeModel>? Employees { get; set; } // Has many Employee

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

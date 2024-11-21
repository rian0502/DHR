using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class ReligionModel
    {
        [Key]
        public int ReligionId { get; set; }
        public string? ReligionName { get; set; }

        //Relationship Models
        public ICollection<EmployeeModel> Employees { get; set; }

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

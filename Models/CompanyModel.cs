using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyID { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public int LocationID { get; set; }

        //Relational Model
        public LocationModel? Location { get; set; } //Belongs to Location
        public ICollection<DepartmentModel>? Departments { get; set; } //Has many Departments

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

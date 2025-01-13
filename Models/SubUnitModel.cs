using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class SubUnitModel
    {
        [Key]
        public int SubUnitId { get; set; }
        public string SubUnitName { get; set; }
        public string SubUnitCode { get; set; }
        public string SubUnitAddress { get; set; }

        public int LocationId { get; set; }
        public int UnitId { get; set; }

        //Relational Model
        public UnitModel? Unit { get; set; } // Belongs to Unit

        public LocationModel? Location { get; set; } // Belongs to Location

        public ICollection<EmployeeModel>? Employees { get; set; } //Has many Employees

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeleteReason { get; set; }
    }
}

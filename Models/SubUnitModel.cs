using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class SubUnitModel
    {
        [Key]
        public int SubUnitID { get; set; }
        public string SubUnitName { get; set; }
        public string SubUnitCode { get; set; }
        public string SubUnitAddress { get; set; }

        public int LocationID { get; set; }
        public int UnitID { get; set; }

        //Relational Model
        public UnitModel? Unit { get; set; } // Belongs to Unit

        public LocationModel? Location { get; set; } // Belongs to Location

        public ICollection<EmployeeModel>? Employees { get; set; } //Has many Employees

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

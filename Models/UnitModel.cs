using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class UnitModel
    {
        [Key]
        public int UnitId { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }

        //Relational Model
        public ICollection<SubUnitModel>? SubUnits { get; set; } //Has many SubUnits

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

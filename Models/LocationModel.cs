using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class LocationModel
    {
        [Key]
        public int LocationId { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        //Relational Model
        public ICollection<CompanyModel>? Companies { get; set; } // Has many companies

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

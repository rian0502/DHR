using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class LocationModel
    {
        [Key]
        public int LocationID { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        //Relational Model
        public ICollection<CompanyModel>? Companies { get; set; } // Has many companies
    }
}

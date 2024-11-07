using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int? LocationID { get; set; }
        public LocationModel LocationModel { get; set; }
    }
}

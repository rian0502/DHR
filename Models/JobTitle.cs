using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class JobTitle
    {
        [Key]   
        public int JobTitleID { get; set; }
        public string? JobTitleName { get; set; }
        public string? JobTitleCode { get; set; }
        public string? JobTitleDescription { get; set; }

    }
}

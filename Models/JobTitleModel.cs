using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class JobTitleModel
    {
        [Key]   
        public int JobTitleID { get; set; }
        public string? JobTitleName { get; set; }
        public string? JobTitleCode { get; set; }
        public string? JobTitleDescription { get; set; }

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

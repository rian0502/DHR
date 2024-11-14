using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class EducationModel
    {
        [Key]
        public int EducationId { get; set; }
        public string? EducationName { get; set; }

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

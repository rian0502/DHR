using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class PeriodModel
    {
        [Key] public int PeriodId { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public bool IsActive { get; set; }

        //Log Attr
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
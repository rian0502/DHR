using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class TaxExemptIncomeModel
    {
        [Key]
        public int TaxExemptIncomeId { get; set; }
        public string? TaxExemptIncomeCode { get; set; }
        public string? TaxExemptIncomeName { get; set; }

        //Relational Models
        public ICollection<EmployeeModel>? Employees { get; set; } // Has many Employee

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class EmployeeModel
    {
        [Key]
        public int? EmployeeID { get; set; }

        public int? DivisionID { get; set; }
        public int? JobTitleID { get; set; }
        public int? ReligionId { get; set; }
        public int? EducationId { get; set; }
        public int? TaxExemptIncomeId { get; set; }
        public string? UserId { get; set; }
        

        //Relational Model
        public DivisionModel? Division { get; set; } // Belongs to Division
        public JobTitleModel? JobTitle { get; set; } // Belongs to JobTitle
        public ReligionModel? Religion { get; set; } // Belongs to Religion
        public EducationModel? Education { get; set; } // Belongs to Education
        public TaxExemptIncomeModel? TaxExemptIncome { get; set; } // Belongs to TaxExemptIncome
        public required Users Users { get; set; } // Belongs to Users
        public ICollection<EmployeeDependentModel>? EmployeeDependents { get; set; } // Has many EmployeeDependent

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

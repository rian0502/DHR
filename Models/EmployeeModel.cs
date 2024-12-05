using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class EmployeeModel
    {
        [Key]
        public int? EmployeeId { get; set; }
        
        public int Nip { get; set; }
        
        public required string Nik { get; set; }
        public required string Npwp { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public int? DivisionId { get; set; }
        public int? JobTitleId { get; set; }
        public int? ReligionId { get; set; }
        public int? EducationId { get; set; }
        public int? TaxExemptIncomeId { get; set; }
        public int? SubUnitId { get; set; }
        public string? UserId { get; set; }
        
        //Relational Model
        public DivisionModel? Division { get; set; } // Belongs to Division
        public JobTitleModel? JobTitle { get; set; } // Belongs to JobTitle
        public ReligionModel? Religion { get; set; } // Belongs to Religion
        public EducationModel? Education { get; set; } // Belongs to Education
        public TaxExemptIncomeModel? TaxExemptIncome { get; set; } // Belongs to TaxExemptIncome
        public SubUnitModel? SubUnit { get; set; } // Belongs to SubUnit
        public Users? Users { get; set; } // Belongs to Users
        public ICollection<EmployeeDependentModel>? EmployeeDependents { get; set; } // Has many EmployeeDependent
        public ICollection<EmployeeBenefit>? Benefits { get; set; } // Has many Benefit

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DHR.Models;

public class EmployeeMedicalClaim
{
    [Key]
    public int EmployeeMedicalClaimId { get; set; }
    public DateOnly? ClaimDate { get; set; }
    public string? Diagnosis { get; set; }
    public string? ClaimStatus { get; set; }
    public string? ClaimCategory { get; set; }
    public string? ClaimDescription { get; set; }
    public DateOnly? PaymentPeriod { get; set; }
    public int PeriodId { get; set; }
    public int EmployeeId { get; set; }
    
    //Relational Model
    public EmployeeModel? Employee { get; set; } // Belongs to Employee
    public PeriodModel? Period { get; set; } // Belongs to Period
    
    //Log
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
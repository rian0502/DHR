using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.ManagementMedicalClaim;

public class EditViewModel
{
    
    [Required(ErrorMessage = "Missing Update Required")]
    public int EmployeeMedicalClaimId { get; set; }
    
    [Required(ErrorMessage = "Claim Date is required")]
    public DateOnly? ClaimDate { get; set; }
    [Required(ErrorMessage = "Payment Period is required")]
    public DateOnly? PaymentPeriod { get; set; }
    [Required(ErrorMessage = "Diagnosis is required")]
    public required string Diagnosis { get; set; }
    [Required(ErrorMessage = "Claim Status is required")]
    public required string ClaimStatus { get; set; }
    [Required(ErrorMessage = "Claim Category is required")]
    public required string ClaimCategory { get; set; }
    [Required(ErrorMessage = "Claim Description is required")]
    public required string ClaimDescription { get; set; }
    [Required(ErrorMessage = "Period is required")]
    public int PeriodId { get; set; }
    [Required(ErrorMessage = "Employee is required")]
    public int EmployeeId { get; set; }
}
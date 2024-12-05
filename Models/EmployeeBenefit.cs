using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models;

public class EmployeeBenefit
{
    [Key]
    public int EmployeeBenefitId { get; set; }
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public int EmployeeId { get; set; }
    public int BenefitId { get; set; }
    public double Amount { get; set; }
    
    public BenefitModel? Benefit { get; set; }
    public EmployeeModel? Employee { get; set; }
    
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.EmployeeBenefit;

public class CreateEmployeeBenefit
{
    [Required(ErrorMessage = "Start Date is required")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Required(ErrorMessage = "End Date is required")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Required(ErrorMessage = "Benefit is required")]
    public int BenefitId { get; set; }
    [Required(ErrorMessage = "Amount is required")]
    public double Amount { get; set; }
    
    public int EmployeeId { get; set; }
}
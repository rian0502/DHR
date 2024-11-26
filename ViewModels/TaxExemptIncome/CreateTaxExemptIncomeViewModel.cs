using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.TaxExemptIncome;

public class CreateTaxExemptIncomeViewModel
{
    [Required(ErrorMessage = "Tax Exempt Income Code is required")]
    public required string TaxExemptIncomeCode { get; set; }
    [Required(ErrorMessage = "Tax Exempt Income Name is required")]
    public required string TaxExemptIncomeName { get; set; }
}
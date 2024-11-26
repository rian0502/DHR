using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.TaxExemptIncome;

public class EditTaxExemptIncomeViewModel
{
    [Required(ErrorMessage = "Missing Required Field")]
    public required int TaxExemptIncomeId { get; set; }
    [Required(ErrorMessage = "Tax Exempt Income Code is required")]
    public required string TaxExemptIncomeCode { get; set; }
    [Required(ErrorMessage = "Tax Exempt Income Name is required")]
    public required string TaxExemptIncomeName { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Company;

public class EditCompanyViewModel
{
    [Required(ErrorMessage = "Missing Attribute")]
    public required int CompanyId { get; set; }
    [Required(ErrorMessage = "Company Code is required")]
    public required string CompanyCode { get; set; }
    [Required(ErrorMessage = "Company Name is required")]
    public required string CompanyName { get; set; }
    [Required(ErrorMessage = "Location is required")]
    public required int LocationId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Company;

public class CreateCompanyViewModel
{
    [Required(ErrorMessage = "Company Code is required")]
    public required string CompanyCode { get; set; }
    [Required(ErrorMessage = "Company Name is required")]
    public required string CompanyName { get; set; }
    [Required(ErrorMessage = "Location is required")]
    public required int LocationId { get; set; }
}
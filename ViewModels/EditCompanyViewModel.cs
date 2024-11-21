using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels;

public class EditCompanyViewModel
{
    [Required(ErrorMessage = "Missing Attribute")]
    public int CompanyID { get; set; }
    [Required(ErrorMessage = "Company Code is required")]
    public string? CompanyCode { get; set; }
    [Required(ErrorMessage = "Company Name is required")]
    public string? CompanyName { get; set; }
    [Required(ErrorMessage = "Location is required")]
    public int? LocationID { get; set; }
}
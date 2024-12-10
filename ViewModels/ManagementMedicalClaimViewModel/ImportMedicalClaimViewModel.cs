using System.ComponentModel.DataAnnotations;
using DAHAR.Validation;

namespace DAHAR.ViewModels.ManagementMedicalClaimViewModel;

public class ImportMedicalClaimViewModel
{
    [Required(ErrorMessage = "Please select a file.")]
    [Extension(".xlsx, .xls")]
    public required IFormFile ExcelFile { get; set; }
}
using System.ComponentModel.DataAnnotations;
using DHR.Validation;

namespace DHR.ViewModels.ManagementMedicalClaimViewModel;

public class ImportMedicalClaimViewModel
{
    [Required(ErrorMessage = "Please select a file.")]
    [Extension(".xlsx, .xls")]
    public required IFormFile ExcelFile { get; set; }
}
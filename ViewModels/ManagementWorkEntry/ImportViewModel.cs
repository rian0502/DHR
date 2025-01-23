using System.ComponentModel.DataAnnotations;
using DHR.Validation;

namespace DHR.ViewModels.ManagementWorkEntry;

public class ImportViewModel
{
    [Required(ErrorMessage = "Please select a file.")]
    [Extension(".xlsx, .xls")]
    [MaxSize(10485760, ErrorMessage = "The file size must be less than or equal to 10MB.")]
    public required IFormFile ExcelFile { get; set; }
}
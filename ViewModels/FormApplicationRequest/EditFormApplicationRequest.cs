using System.ComponentModel.DataAnnotations;
using DHR.Validation;

namespace DHR.ViewModels.FormApplicationRequest;

public class EditFormApplicationRequest
{
    public int IdForm { get; set; }
    [Required(ErrorMessage = "Code is required")]
    public required string FormCode { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public required string FormName { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public required string Description { get; set; }
    
    public string? FormPath { get; set; }
    
    
    [Extension(".pdf")]
    public IFormFile? File { get; set; }
}
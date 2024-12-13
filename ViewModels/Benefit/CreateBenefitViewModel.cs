using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Benefit;

public class CreateBenefitViewModel
{
    [Required(ErrorMessage = "Benefit Name is required")]
    public string BenefitName { get; set; }

    [Required(ErrorMessage = "Benefit Description is required")]
    public string BenefitDescription { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; }

    public bool IsActive { get; set; }
    public bool IsMonetary { get; set; }
}
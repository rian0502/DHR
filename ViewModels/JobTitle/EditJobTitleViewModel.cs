using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.JobTitle
{
    public class EditJobTitleViewModel
    {
        [Required(ErrorMessage ="Missing Attribute")]
        public int JobTitleId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? JobTitleName { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public string? JobTitleCode { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? JobTitleDescription { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels
{
    public class CreateJobTitleViewModel
    {
        [Required(ErrorMessage = "Job Title Name is required")]
        public string JobTitleName { get; set; }
        [Required(ErrorMessage = "Job Title Code is required")]
        public string JobTitleCode { get; set; }
        [Required(ErrorMessage = "Job Description is required")]
        public string JobTitleDescription { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
    }
}

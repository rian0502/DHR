using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public required string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}

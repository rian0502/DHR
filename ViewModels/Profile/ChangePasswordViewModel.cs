using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Profile;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public required string OldPassword { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    public required string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public required string ConfirmNewPassword { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Employee;

public class EditAccountEmployeeViewModel
{
    [Required(ErrorMessage = "UserID is required")]
    public string UserID { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    public string? Password { get; set; }
    
    public DateTimeOffset? LockoutEnd { get; set; }
    
}
using Microsoft.AspNetCore.Identity;


namespace DAHAR.Models
{
    public class Users : IdentityUser
    {
        public required string FullName { get; set; }
    }
}

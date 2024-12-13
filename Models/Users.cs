using Microsoft.AspNetCore.Identity;


namespace DHR.Models
{
    public class Users : IdentityUser
    {
        public required string FullName { get; set; }
    }
}

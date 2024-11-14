using Microsoft.AspNetCore.Identity;


namespace Presensi360.Models
{
    public class Users : IdentityUser
    {
        public required string FullName { get; set; }
    }
}

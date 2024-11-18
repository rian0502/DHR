using Microsoft.AspNetCore.Identity;
using Presensi360.Helper;
using Presensi360.Models;

namespace Presensi360.Providers
{
    public class EmployeeService(AppDBContext context, UserManager<Users> userManager)
    {
        private readonly AppDBContext _context = context;
        private readonly UserManager<Users> _userManager = userManager;


       
    }
}

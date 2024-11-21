using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.AspNetCore.Identity;

namespace DAHAR.Providers
{
    public class EmployeeService(AppDBContext context, UserManager<Users> userManager)
    {
        private readonly AppDBContext _context = context;
        private readonly UserManager<Users> _userManager = userManager;


       
    }
}

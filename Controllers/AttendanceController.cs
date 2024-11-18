using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presensi360.Helper;
using Presensi360.Models;
using Presensi360.Providers;

namespace Presensi360.Controllers
{
    [Authorize]
    public class AttendanceController(PeriodService periodService, UserManager<Users> userManager, AppDBContext context) : Controller
    {
        private readonly PeriodService _periodService = periodService;
        private readonly UserManager<Users> _userManager = userManager;
        private readonly AppDBContext _context = context;

        public async Task<IActionResult> Index()
        {
            ViewBag.Periods = await _periodService.FindAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAttendanceAsync(int periodId)
        {

            var user = await userManager.GetUserAsync(User);
            var employee = await _context.Employee
                                     .FirstOrDefaultAsync(e => e.UserId == user.Id);
            return Ok(new
            {
                Status = true,
                Message = "Success",
                User = user,
                Employee = employee
            });
        }
    }
}

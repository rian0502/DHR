using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Controllers
{
    [Authorize(Roles = "User")]
    public class AttendanceController : Controller
    {
        private readonly PeriodService _periodService;
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context;
        private readonly AttendanceService _attendanceService;

        public AttendanceController(PeriodService periodService, UserManager<Users> userManager, AppDbContext context,
            AttendanceService attendanceService)
        {
            _periodService = periodService;
            _userManager = userManager;
            _context = context;
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Index()
        {
            var periods = await _periodService.FindAll();
            var periodModels = periods.ToList();
            ViewBag.Periods = new SelectList(
                periodModels.Select(period => new
                {
                    Value = period.PeriodId,
                    Text = $"{period.StartPeriodDate:dd MMMM} - {period.EndPeriodDate:dd MMMM}"
                }),
                "Value",
                "Text",
                periodModels.FirstOrDefault(p => p.IsActive)?.PeriodId
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAttendanceAsync(int periodId, int year = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employee == null)
            {
                return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
            }

            var attendance = year == 0
                ? _attendanceService.GetAttendance(employee.Nip, periodId)
                : _attendanceService.GetAttendance(employee.Nip, periodId, year);

            return Ok(new
            {
                Status = true,
                Message = "Success",
                Employee = employee,
                Attendance = attendance
            });
        }
    }
}
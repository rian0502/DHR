using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class AttendanceController(
        PeriodService periodService,
        UserManager<Users> userManager,
        AppDbContext context,
        AttendanceService attendanceService)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            var periods = await periodService.AttendancePeriod();
            var periodModels = periods.ToList();
            var activePeriod = periodModels.FirstOrDefault(p => p.IsActive);

            ViewBag.Periods = new SelectList(
                periodModels.Select(period => new
                {
                    Value = period.PeriodId,
                    Text = $"{period.StartPeriodDate:dd MMMM yyyy} - {period.EndPeriodDate:dd MMMM yyyy}"
                }),
                "Value",
                "Text",
                activePeriod?.PeriodId
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAttendanceData(int periodId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var employee = await context.Employee.Select(e => new { e.Nip, e.UserId })
                .Where(e => e.UserId == user.Id).FirstOrDefaultAsync();
            if (employee == null)
            {
                return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
            }

            var attendance = await attendanceService.GetAttendance(employee.Nip, periodId);
            return Ok(new
            {
                Status = true,
                Message = "Success",
                Attendance = attendance
            });
        }
    }
}
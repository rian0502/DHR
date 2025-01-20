using DHR.Helper;
using DHR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class LeaveRequestController(AppDbContext context, UserManager<Users> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var employee = await context.Employee
                .Include(l => l.EmployeeLeaveRequestModels)
                .FirstOrDefaultAsync(e => user != null && e.UserId == user.Id);
            if (employee == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var filter = employee?.EmployeeLeaveRequestModels?
                .Where(el => el.IsDeleted == false)
                .ToList();
            return View(filter);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadForm()
        {
            var file = await context.FormApplication.FindAsync(2);
            if (file == null)
            {
                return NotFound("File tidak ditemukan.");
            }
            var fileName = file.PathForm;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File tidak ditemukan.");
            }
            var downloadName = $"{file.FormName}.pdf";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", downloadName);
        }
    }
}

using DHR.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class OpticalServicesController(AppDbContext context) : Controller
    {
        // GET: OpticalServicesController
        public async Task<ActionResult> Index()
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => User.Identity != null && x.UserName == User.Identity.Name);
            if (user != null)
            {
                var employee = await context.Employee
                    .FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (employee != null)
                {
                    var medicalClaims = await context.EmployeeMedicalClaims
                        .Include(p => p.Period)
                        .Where(x => x.EmployeeId == employee.EmployeeId && x.ClaimCategory == "KACAMATA" && x.IsDeleted == false)
                        .ToListAsync();

                    return View(medicalClaims);
                }
            }

            return NotFound("User or Employee not found.");
        }

        // GET: OpticalServicesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OpticalServicesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpticalServicesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OpticalServicesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OpticalServicesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OpticalServicesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OpticalServicesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
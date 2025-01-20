using DHR.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class InpatientController(AppDbContext context) : Controller
    {
        // GET: InpatientController
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
                        .Where(x => x.EmployeeId == employee.EmployeeId && x.ClaimCategory == "RAWAT_INAP" && x.IsDeleted == false)
                        .ToListAsync();

                    return View(medicalClaims);
                }
            }

            return View();
        }
        
        // GET: InpatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InpatientController/Create
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

        // GET: InpatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InpatientController/Edit/5
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
        
        // POST: InpatientController/Delete/5
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
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

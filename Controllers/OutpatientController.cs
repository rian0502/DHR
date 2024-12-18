using DHR.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class OutpatientController(AppDbContext context) : Controller
    {
        // GET: OutpatientController
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
                        .Where(x => x.EmployeeId == employee.EmployeeId && x.ClaimCategory == "RAWAT_JALAN")
                        .ToListAsync();

                    return View(medicalClaims);
                }
            }
            
            return NotFound("User or Employee not found.");
        }


        // GET: OutpatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OutpatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutpatientController/Create
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
        // GET: OutpatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OutpatientController/Edit/5
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

        // GET: OutpatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OutpatientController/Delete/5
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
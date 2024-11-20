using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeDependentController : Controller
    {
        // GET: EmployeeDependentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeDependentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeDependentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeDependentController/Create
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

        // GET: EmployeeDependentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeDependentController/Edit/5
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

        // GET: EmployeeDependentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeDependentController/Delete/5
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

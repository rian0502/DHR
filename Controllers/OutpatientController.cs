using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    [Authorize(Roles = "User")]
    public class OutpatientController : Controller
    {
        // GET: OutpatientController
        public ActionResult Index()
        {
            return View();
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

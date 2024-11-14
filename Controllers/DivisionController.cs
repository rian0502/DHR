using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class DivisionController : Controller
    {
        // GET: DivisionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DivisionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DivisionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DivisionController/Create
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

        // GET: DivisionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DivisionController/Edit/5
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

        // GET: DivisionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DivisionController/Delete/5
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

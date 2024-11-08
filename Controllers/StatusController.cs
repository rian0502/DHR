using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class StatusController : Controller
    {
        // GET: StatusController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StatusController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatusController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatusController/Create
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

        // GET: StatusController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatusController/Edit/5
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

        // GET: StatusController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatusController/Delete/5
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

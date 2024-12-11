using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class OpticalServicesController : Controller
    {
        // GET: OpticalServicesController
        public ActionResult Index()
        {
            return View();
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

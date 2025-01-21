using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers
{
    public class ManagementWorkEntryController : Controller
    {
        // GET: ManagementWorkEntryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementWorkEntryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagementWorkEntryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementWorkEntryController/Create
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

        // GET: ManagementWorkEntryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagementWorkEntryController/Edit/5
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

        // GET: ManagementWorkEntryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementWorkEntryController/Delete/5
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    [Authorize]
    public class SubUnitController : Controller
    {
        // GET: SubUnitController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubUnitController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubUnitController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubUnitController/Create
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

        // GET: SubUnitController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubUnitController/Edit/5
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

        // GET: SubUnitController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubUnitController/Delete/5
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

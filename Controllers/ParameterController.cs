using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class ParameterController : Controller
    {
        // GET: ParameterController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ParameterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParameterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParameterController/Create
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

        // GET: ParameterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParameterController/Edit/5
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

        // GET: ParameterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParameterController/Delete/5
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class MaternityServicesController : Controller
    {
        // GET: MaternityServicesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaternityServicesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MaternityServicesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaternityServicesController/Create
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

        // GET: MaternityServicesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MaternityServicesController/Edit/5
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

        // GET: MaternityServicesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MaternityServicesController/Delete/5
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

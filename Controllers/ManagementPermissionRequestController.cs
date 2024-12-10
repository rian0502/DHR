using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class ManagementPermissionRequestController : Controller
    {
        // GET: ManagementPermissionRequestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementPermissionRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagementPermissionRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementPermissionRequestController/Create
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

        // GET: ManagementPermissionRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagementPermissionRequestController/Edit/5
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

        // GET: ManagementPermissionRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementPermissionRequestController/Delete/5
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

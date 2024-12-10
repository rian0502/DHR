using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class ManagementLeaveRequestController : Controller
    {
        // GET: ManagementLeaveRequest
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementLeaveRequest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagementLeaveRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementLeaveRequest/Create
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

        // GET: ManagementLeaveRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagementLeaveRequest/Edit/5
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

        // GET: ManagementLeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementLeaveRequest/Delete/5
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

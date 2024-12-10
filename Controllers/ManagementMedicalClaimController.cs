using DAHAR.ViewModels.ManagementMedicalClaimViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class ManagementMedicalClaimController : Controller
    {
        // GET: ManagementClaimController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementClaimController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagementClaimController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementClaimController/Create
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

        // GET: ManagementClaimController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagementClaimController/Edit/5
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

        // GET: ManagementClaimController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementClaimController/Delete/5
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

        public async Task<IActionResult> Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(ImportMedicalClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return Ok(model);
        }
    }
}

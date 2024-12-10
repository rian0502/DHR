using DAHAR.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class MedicalClaimController(AppDbContext context) : Controller
    {
        // GET: MedicalClaimController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadForm()
        {
            var file = await context.FormApplication.FindAsync(1);
            if (file == null)
            {
                TempData["Message"] = "File Not Found";
                return RedirectToAction(nameof(Index));
            }
            var fileName = file.PathForm;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                TempData["Message"] = "File Not Found";
                return RedirectToAction(nameof(Index));
            }
            var downloadName = $"{file.FormName}.pdf";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", downloadName);
        }


        // GET: MedicalClaimController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MedicalClaimController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalClaimController/Create
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

        // GET: MedicalClaimController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MedicalClaimController/Edit/5
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

        // GET: MedicalClaimController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MedicalClaimController/Delete/5
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

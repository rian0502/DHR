using DHR.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers
{
    [Authorize(Roles = "User")]
    public class BusinessTravelRequestController(AppDbContext context) : Controller
    {
        // GET: BusinessTravelRequestController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadForm()
        {
            var file = await context.FormApplication.FindAsync(3);
            if (file == null)
            {
                return NotFound("File tidak ditemukan.");
            }
            var fileName = file.PathForm;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File tidak ditemukan.");
            }
            var downloadName = $"{file.FormName}.pdf";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", downloadName);
        }
        // GET: BusinessTravelRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BusinessTravelRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessTravelRequestController/Create
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

        // GET: BusinessTravelRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BusinessTravelRequestController/Edit/5
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

        // GET: BusinessTravelRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BusinessTravelRequestController/Delete/5
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

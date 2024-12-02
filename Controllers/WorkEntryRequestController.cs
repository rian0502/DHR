using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class WorkEntryRequestController : Controller
    {
        // GET: WorkEntryRequest
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DownloadForm()
        {
            var fileName = "permintaan_ijin_masuk_kerja.pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File tidak ditemukan.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }

        // GET: WorkEntryRequest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkEntryRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkEntryRequest/Create
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

        // GET: WorkEntryRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkEntryRequest/Edit/5
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

        // GET: WorkEntryRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkEntryRequest/Delete/5
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
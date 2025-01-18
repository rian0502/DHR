using DHR.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers;

[Authorize(Roles = "User")]
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
            TempData["Errors"] = "File Not Found";
            return RedirectToAction(nameof(Index), "Outpatient");
        }

        var fileName = file.PathForm;
        if (fileName != null)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                TempData["Errors"] = "File Not Found";
                return RedirectToAction(nameof(Index));
            }

            var downloadName = $"{file.FormName}.pdf";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", downloadName);
        }
        else
        {
            TempData["Errors"] = "File Not Found";
            return RedirectToAction(nameof(Index), "Outpatient");
        }
    }
    
}
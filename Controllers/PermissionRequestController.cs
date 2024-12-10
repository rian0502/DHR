using DAHAR.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

public class PermissionRequestController(AppDbContext context) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DownloadForm()
    {
        var file = await context.FormApplication.FindAsync(2);
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
}
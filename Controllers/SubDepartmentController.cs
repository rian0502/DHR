using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers;

public class SubDepartmentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
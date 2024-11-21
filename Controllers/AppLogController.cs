using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

public class AppLogController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
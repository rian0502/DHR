using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

public class FormApplicationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
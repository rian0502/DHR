using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
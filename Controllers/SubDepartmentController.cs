using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class SubDepartmentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
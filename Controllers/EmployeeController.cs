using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

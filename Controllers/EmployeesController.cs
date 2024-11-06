using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

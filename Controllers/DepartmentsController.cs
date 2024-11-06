using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

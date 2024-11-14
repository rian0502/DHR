using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

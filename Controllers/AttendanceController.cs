using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

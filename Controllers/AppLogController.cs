using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class AppLogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

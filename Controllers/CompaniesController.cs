using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

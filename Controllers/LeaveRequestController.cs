using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class LeaveRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

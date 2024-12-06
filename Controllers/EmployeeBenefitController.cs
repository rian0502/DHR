using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers
{
    public class EmployeeBenefitController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }
    }
}

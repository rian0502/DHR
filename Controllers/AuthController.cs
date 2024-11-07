using Microsoft.AspNetCore.Mvc;
using Presensi360.Models.ViewModels;
using System.Transactions;

namespace Presensi360.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel form)
        {

            if (ModelState.IsValid)
            {
                return Json(new {
                    form = form
                });
            }

            return View(form);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

    [Authorize(Roles = "Admin")]
    public class SubUnitController : Controller
    {
        // GET: SubUnitController
        public ActionResult Index()
        {
            return View();
        }
    }


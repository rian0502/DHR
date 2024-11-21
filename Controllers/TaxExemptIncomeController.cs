using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class TaxExemptIncomeController : Controller
{
    // GET: TaxExemptIncomeController
    public ActionResult Index()
    {
        return View();
    }
}

        


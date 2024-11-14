using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presensi360.Controllers
{
    public class TaxExemptIncomeController : Controller
    {
        // GET: TaxExemptIncomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TaxExemptIncomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaxExemptIncomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaxExemptIncomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaxExemptIncomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaxExemptIncomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaxExemptIncomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaxExemptIncomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

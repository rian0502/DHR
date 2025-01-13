using DHR.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DHR.Controllers
{
    public class RolesController(AppDbContext context, RoleManager<IdentityRole> roleManager) : Controller
    {
        // GET: RolesController
        public async Task<ActionResult> Index()
        {
            var roles = await context.Roles.Where(r => r.Name != "Admin").OrderBy(r => r.Name).ToListAsync();
            return View(roles);
        }
        
        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                string roleName = collection["RoleName"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(roleName))
                {
                    ModelState.AddModelError("RoleName", "Role Name is Required");
                    return View(collection);
                }

                if (await roleManager.RoleExistsAsync(roleName))
                {
                    ModelState.AddModelError("RoleName", "Role Name already exists");
                    return View(collection);
                }
                await roleManager.CreateAsync(new IdentityRole(roleName));
                TempData["Success"] = "Role Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return View();
            }
        }
    }
}

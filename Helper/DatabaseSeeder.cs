using DHR.Models;
using Microsoft.AspNetCore.Identity;

namespace DHR.Helper
{
    public class DatabaseSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            string[] roles = { "Admin", "User", "AttendanceAdministrator", "ClaimAdministrator" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new Users
            {
                UserName = "admin",
                Email = "admin@darmexagro.com",
                FullName = "Administrator",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "[HRD-IT_APPLICATION(2025){Attendance}]");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

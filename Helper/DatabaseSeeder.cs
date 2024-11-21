using DAHAR.Models;
using Microsoft.AspNetCore.Identity;

namespace DAHAR.Helper
{
    public class DatabaseSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
            var dbContext = serviceProvider.GetRequiredService<AppDBContext>();

            string[] roles = { "Admin", "User" };
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
                var result = await userManager.CreateAsync(adminUser, "1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }


            var normalUser = new Users
            {
                UserName = "muhammadhasibuan",
                Email = "muhammad.hasibuan@darmexagro.com",
                FullName = "Muhammad Febrian Hasibuan",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(normalUser.Email) == null)
            {
                var result = await userManager.CreateAsync(normalUser, "1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }


            if (!dbContext.Employee.Any())
            {
                EmployeeModel employee = new()
                {
                    EmployeeID = 7767,
                    NIK = "321123",
                    NPWP = "955630603435000",
                    Gender = "M",
                    Address = "Bekasi",
                    JoinDate = DateTime.Parse("7/8/2024"),
                    DivisionID = 1,
                    JobTitleID = 33,
                    ReligionId = 1,
                    EducationId = 7,
                    TaxExemptIncomeId = 1,
                    SubUnitId = 1,
                    UserId = normalUser.Id,
                    CreatedBy = "admin",
                    CreatedAt = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedAt = DateTime.Now
                };

                dbContext.Employee.Add(employee);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

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
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            //string[] roles = { "Admin", "User" };
            //foreach (var role in roles)
            //{
            //    if (!await roleManager.RoleExistsAsync(role))
            //    {
            //        await roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}

            //var adminUser = new Users
            //{
            //    UserName = "admin",
            //    Email = "admin@darmexagro.com",
            //    FullName = "Administrator",
            //    EmailConfirmed = true
            //};

            //if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            //{
            //    var result = await userManager.CreateAsync(adminUser, "1234");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(adminUser, "Admin");
            //    }
            //}


            //var normalUser = new Users
            //{
            //    UserName = "muhammadhasibuan",
            //    Email = "muhammad.hasibuan@darmexagro.com",
            //    FullName = "Muhammad Febrian Hasibuan",
            //    PhoneNumber = "0895615114767",
            //    PhoneNumberConfirmed = true,
            //    EmailConfirmed = true
            //};

            //if (await userManager.FindByEmailAsync(normalUser.Email) == null)
            //{
            //    var result = await userManager.CreateAsync(normalUser, "1234");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(normalUser, "User");
            //        normalUser = await userManager.FindByEmailAsync(normalUser.Email);
            //    }
            //}


            if (!dbContext.Employee.Any())
            {
                try
                {
                    EmployeeModel employee = new()
                    {
                        Nip = 7767,
                        Nik = "3211234567890123",
                        Npwp = "955630603435000",
                        Gender = "M",
                        Address = "Bekasi",
                        JoinDate = DateTime.Parse("7/8/2024"),
                        DivisionId = 1,
                        JobTitleId = 8,
                        ReligionId = 1,
                        EducationId = 7,
                        TaxExemptIncomeId = 1,
                        SubUnitId = 6,
                        UserId = "7a0cdb6e-094f-4466-827e-2bc63c47156f",
                        CreatedBy = "admin",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedBy = "admin",
                        UpdatedAt = DateTime.UtcNow
                    };

                    dbContext.Employee.Add(employee);
                    await dbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
        }
    }
}

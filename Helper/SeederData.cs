using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presensi360.Models;

namespace Presensi360.Helper
{
    public class SeederData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            var context = serviceProvider.GetRequiredService<AppDBContext>();


            var roleNames = new string[] { "Admin", "User", "Manager" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var users = new[]
            {
                new { UserName = "admin@domain.com", Email = "admin@domain.com", Password = "Admin123", Role = "Admin" },
                new { UserName = "user@domain.com", Email = "user@domain.com", Password = "User123", Role = "User" },
                new { UserName = "manager@domain.com", Email = "manager@domain.com", Password = "Manager123", Role = "Manager" }
            };
            foreach (var userInfo in users)
            {
                var userExists = await userManager.FindByEmailAsync(userInfo.Email);
                if (userExists == null)
                {
                    var user = new Users
                    {
                        UserName = userInfo.UserName,
                        Email = userInfo.Email,
                    };
                    var createUserResult = await userManager.CreateAsync(user, userInfo.Password);
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                }
            }
            await LocationAsync(context);
            await CompanyAsync(context);
            await DepartmentAsync(context);
            await SubDepartmentAsync(context);
            await JobTitlesAsync(context);
            await EmployeesAsync(context, userManager);
            await context.SaveChangesAsync();
        }

        private static async Task LocationAsync(AppDBContext context)
        {
            var locations = new[]
            {
                new { LocationCode = "MDN", LocationName = "MEDAN" },
                new { LocationCode = "JKT", LocationName = "JAKARTA" },
                new { LocationCode = "BKS", LocationName = "BEKASI" },
                new { LocationCode = "RIW", LocationName = "RIAU" },
                new { LocationCode = "PKN", LocationName = "PEKANBARU" },
                new { LocationCode = "TJP", LocationName = "TANJUNG PINANG" }
            };

            foreach (var loc in locations)
            {
                if (!await context.Locations.AnyAsync(l => l.LocationCode == loc.LocationCode))
                {
                    context.Locations.Add(new LocationModel
                    {
                        LocationCode = loc.LocationCode,
                        LocationName = loc.LocationName
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private static async Task CompanyAsync(AppDBContext context)
        {
            var companies = new[]
            {
                new { CompanyCode = "DPN", CompanyName = "DUTAPALMA NUSANTARA, PT", LocationCode = "JKT" },
                new { CompanyCode = "JS", CompanyName = "JOHAN SENTOSA, PT", LocationCode = "BKS" },
                new { CompanyCode = "APN", CompanyName = "ADITYAPALMA NUSANTARA, PT", LocationCode = "RIW" },
                new { CompanyCode = "KAT", CompanyName = "KENCANA AMAL TANI, PT", LocationCode = "PKN" },
                new { CompanyCode = "EMA", CompanyName = "ELUAN MAHKOTA, PT", LocationCode = "TJP" },
                new { CompanyCode = "CSB", CompanyName = "CERENTI SUBUR, PT", LocationCode = "MDN" }
            };

            foreach (var company in companies)
            {
                var location = await context.Locations.FirstOrDefaultAsync(l => l.LocationCode == company.LocationCode);
                if (location != null)
                {
                    var existingCompany = await context.Companies.FirstOrDefaultAsync(c => c.CompanyCode == company.CompanyCode);
                    if (existingCompany == null)
                    {
                        context.Companies.Add(new CompanyModel
                        {
                            CompanyCode = company.CompanyCode,
                            CompanyName = company.CompanyName,
                            LocationID = location.LocationID
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }

        private static async Task DepartmentAsync(AppDBContext context)
        {
            var departments = new[]
            {
                new { DepartmentName = "ACCOUNTING", DepartmentCode = "ACT" },
                new { DepartmentName = "AGRONOMI DEPT", DepartmentCode = "AGR" },
                new { DepartmentName = "ANGKUTAN", DepartmentCode = "ANK" },
                new { DepartmentName = "AUDIT", DepartmentCode = "AUD" },
                new { DepartmentName = "BUILDING MANAGEMENT", DepartmentCode = "BLD" },
                new { DepartmentName = "COST CONTROL", DepartmentCode = "CC" },
                new { DepartmentName = "CAFE", DepartmentCode = "CF" },
                new { DepartmentName = "CORPORATE SEC.", DepartmentCode = "COR" },
                new { DepartmentName = "ENGINEERING DEPT.", DepartmentCode = "ENG" },
                new { DepartmentName = "EXPORT", DepartmentCode = "EXP" },
                new { DepartmentName = "FINANCE DEPT.", DepartmentCode = "FIN" },
                new { DepartmentName = "GUDANG", DepartmentCode = "GDG" },
                new { DepartmentName = "HRD & GA", DepartmentCode = "HRD" },
                new { DepartmentName = "HUMAS DEPT.", DepartmentCode = "HUM" },
                new { DepartmentName = "INSURANCE", DepartmentCode = "INS" },
                new { DepartmentName = "IT DEPT.", DepartmentCode = "IT" }
            };

            foreach (var dept in departments)
            {
                var existingDepartment = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == dept.DepartmentCode);
                if (existingDepartment == null)
                {
                    context.Departments.Add(new Department
                    {
                        DepartmentName = dept.DepartmentName,
                        DepartmentCode = dept.DepartmentCode
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private static async Task SubDepartmentAsync(AppDBContext context)
        {
            var subDepartments = new[]
                {
            new { SubDepartmentName = "Accounting Team A", SubDepartmentCode = "ACTA", DepartmentCode = "ACT" },
            new { SubDepartmentName = "Accounting Team B", SubDepartmentCode = "ACTB", DepartmentCode = "ACT" },
            new { SubDepartmentName = "Agronomi Section 1", SubDepartmentCode = "AGR1", DepartmentCode = "AGR" },
            new { SubDepartmentName = "Agronomi Section 2", SubDepartmentCode = "AGR2", DepartmentCode = "AGR" },
            new { SubDepartmentName = "Transport Team A", SubDepartmentCode = "ANKA", DepartmentCode = "ANK" },
        };

            foreach (var subDept in subDepartments)
            {
                var department = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == subDept.DepartmentCode);

                if (department != null)
                {
                    var existingSubDepartment = await context.SubDepartments.FirstOrDefaultAsync(sd => sd.SubDepartmentCode == subDept.SubDepartmentCode);
                    if (existingSubDepartment == null)
                    {
                        context.SubDepartments.Add(new SubDepartment
                        {
                            SubDepartmentName = subDept.SubDepartmentName,
                            SubDepartmentCode = subDept.SubDepartmentCode,
                            DepartmentID = department.DepartmentID
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task JobTitlesAsync(AppDBContext context)
        {
            var jobTitles = new[]
            {
                new { JobTitleName = "Staff Programmer", JobTitleCode = "SPR", JobTitleDescription = "Software and system programmer staff" },
                new { JobTitleName = "Head of Agronomi Kalbar", JobTitleCode = "HOA", JobTitleDescription = "Head of agronomy in Kalimantan Barat" },
                new { JobTitleName = "Asisten Manager Power Plant", JobTitleCode = "PW", JobTitleDescription = "Assistant Manager for power plant operations" },
                new { JobTitleName = "Staff PC", JobTitleCode = "PC", JobTitleDescription = "Personal computer or tech support staff" },
                new { JobTitleName = "Driver Pribadi", JobTitleCode = "DRPR", JobTitleDescription = "Personal driver for executives" },
                new { JobTitleName = "Driver Operasional", JobTitleCode = "DROP", JobTitleDescription = "Operational driver for company transportation" },
                new { JobTitleName = "Manager Sustainability", JobTitleCode = "MS", JobTitleDescription = "Manager responsible for sustainability programs" },
                new { JobTitleName = "OJT Staff Head of Agronomi", JobTitleCode = "OJTSHA", JobTitleDescription = "On-the-job trainee for head of agronomy" },
                new { JobTitleName = "Manager SAP & IT", JobTitleCode = "MSAIT", JobTitleDescription = "Manager for SAP and IT department" },
                new { JobTitleName = "Manager Administrasi Agronomi", JobTitleCode = "MAA", JobTitleDescription = "Administrative manager in agronomy" },
                new { JobTitleName = "Trainee Asisten Mill Engineering", JobTitleCode = "TAME", JobTitleDescription = "Trainee assistant in mill engineering" },
                new { JobTitleName = "Staff Compensation & Benefit (Temporary)", JobTitleCode = "SCBT", JobTitleDescription = "Temporary staff for compensation and benefits" },
                new { JobTitleName = "Staff Tax Dispute (Temporary)", JobTitleCode = "STDT", JobTitleDescription = "Temporary staff for handling tax disputes" }
            };

            foreach (var jobTitle in jobTitles)
            {
                var existingJobTitle = await context.JobTitles.FirstOrDefaultAsync(jt => jt.JobTitleCode == jobTitle.JobTitleCode);
                if (existingJobTitle == null)
                {
                    context.JobTitles.Add(new JobTitle
                    {
                        JobTitleName = jobTitle.JobTitleName,
                        JobTitleCode = jobTitle.JobTitleCode,
                        JobTitleDescription = jobTitle.JobTitleDescription
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private static async Task EmployeesAsync(AppDBContext context, UserManager<Users> userManager)
        {
            var employees = new[]
            {
                new { EmployeeName = "John Doe", UserEmail = "user@domain.com", JobTitleCode = "SPR", CompanyCode = "DPN", SubDepartmentCode = "ACTA" },
                new { EmployeeName = "Jane Smith", UserEmail = "manager@domain.com", JobTitleCode = "MSAIT", CompanyCode = "JS", SubDepartmentCode = "AGR1" },
                new { EmployeeName = "Michael Johnson", UserEmail = "admin@domain.com", JobTitleCode = "HOA", CompanyCode = "APN", SubDepartmentCode = "BLD" }
            };

            foreach (var employee in employees)
            {
                var user = await userManager.FindByEmailAsync(employee.UserEmail);
                if (user == null)
                {
                    continue;
                }
                var jobTitle = await context.JobTitles.FirstOrDefaultAsync(jt => jt.JobTitleCode == employee.JobTitleCode);
                var company = await context.Companies.FirstOrDefaultAsync(c => c.CompanyCode == employee.CompanyCode);
                var subDepartment = await context.SubDepartments.FirstOrDefaultAsync(sd => sd.SubDepartmentCode == employee.SubDepartmentCode);

                if (jobTitle != null && company != null && subDepartment != null)
                {
                    var existingEmployee = await context.Employee.FirstOrDefaultAsync(e => e.UserId == user.Id && e.JobTitleID == jobTitle.JobTitleID);
                    if (existingEmployee == null)
                    {
                        context.Employee.Add(new Employee
                        {
                            EmployeeName = employee.EmployeeName,
                            UserId = user.Id,
                            JobTitleID = jobTitle.JobTitleID,
                            CompanyID = company.CompanyID,
                            SubDepartmentID = subDepartment.SubDepartmentID
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }

    }
}

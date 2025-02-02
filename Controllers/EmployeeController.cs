﻿using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.Employee;
using DHR.ViewModels.ManagementImportViewModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(
    AppDbContext context,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext,
    EmployeeService employeeService,
    DivisionService divisionService,
    JobTitleService jobTitleService
)
    : Controller
{
    // GET: EmployeeController/Index
    public async Task<IActionResult> Index()
    {
        var employees = await employeeService.FindAll();
        return View(employees);
    }

    // GET: EmployeeController/Details/id
    public async Task<IActionResult> Details(int id)
    {
        var employee = await employeeService.FindById(id);
        return View(employee);
    }

    //GET: EmployeeController/Create

    public async Task<IActionResult> Create()
    {
        ViewBag.Divisions = await divisionService.FindAll();
        ViewBag.JobTitle = await jobTitleService.FindAll();
        ViewBag.Companies = await context.Companies.Where(c => c.IsDeleted == false).ToListAsync();
        ViewBag.Taxes = await context.TaxExemptIncomes.Where(t => t.IsDeleted == false).ToListAsync();
        ViewBag.SubUnits = await context.SubUnits.Where(su => su.IsDeleted == false)
            .Include(su => su.Unit).ToListAsync();
        ViewBag.Religions = await context.Religions.Where(r => r.IsDeleted == false).ToListAsync();
        ViewBag.Educations = await context.Educations.Where(ed => ed.IsDeleted == false).ToListAsync();

        return View();
    }

    // POST: EmployeeController/Create
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Divisions = await divisionService.FindAll();
            ViewBag.JobTitle = await jobTitleService.FindAll();
            ViewBag.Companies = await context.Companies.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Taxes = await context.TaxExemptIncomes.Where(t => t.IsDeleted == false).ToListAsync();
            ViewBag.SubUnits = await context.SubUnits.Where(su => su.IsDeleted == false)
                .Include(su => su.Unit).ToListAsync();
            ViewBag.Religions = await context.Religions.Where(r => r.IsDeleted == false).ToListAsync();
            ViewBag.Educations = await context.Educations.Where(ed => ed.IsDeleted == false).ToListAsync();
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Logout", "Account");
        }

        var timestamp = DateTime.UtcNow;
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var password = model.Nip;
            if (model.Nip.Length < 4)
            {
                password = $"00{model.Nip}";
            }

            var createUser = await userManager.CreateAsync(new Users
            {
                UserName = GenerateUsername(model.Email),
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            }, password);
            //asign role

            if (!createUser.Succeeded)
            {
                var errors = string.Join(", ", createUser.Errors.Select(x => x.Description));
                TempData["Errors"] = errors;
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Create));
            }

            var newUser = await userManager.FindByEmailAsync(model.Email);
            if (newUser == null)
            {
                TempData["Errors"] = "User not found";
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Create));
            }

            await userManager.AddToRoleAsync(newUser, "User");
            await context.Employee.AddAsync(new EmployeeModel
            {
                Nip = int.Parse(model.Nip),
                Nik = model.Nik,
                Npwp = model.Npwp,
                Gender = model.Gender,
                Address = model.Address,
                JoinDate = model.JoinDate,
                CompanyId = model.CompanyId,
                DivisionId = model.DivisionId,
                JobTitleId = model.JobTitleId,
                ReligionId = model.ReligionId,
                EducationId = model.EducationId,
                TaxExemptIncomeId = model.TaxExemptIncomeId,
                SubUnitId = model.SubUnitId,
                UserId = newUser.Id,
                CreatedBy = user.Id,
                CreatedAt = timestamp,
                UpdatedBy = user.Id,
                UpdatedAt = timestamp
            });
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            TempData["Success"] = $"Employee: {newUser.FullName} has been created";
            var logs = new AppLogModel
            {
                CreatedAt = timestamp,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Params = JsonConvert.SerializeObject(model),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeController",
                    Action = "Create",
                    Database = "Employee & AspNetUsers"
                }),
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["Errors"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }


    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Divisions = await divisionService.FindAll();
        ViewBag.JobTitle = await jobTitleService.FindAll();
        ViewBag.Companies = await context.Companies.Where(c => c.IsDeleted == false).ToListAsync();
        ViewBag.Taxes = await context.TaxExemptIncomes.Where(t => t.IsDeleted == false).ToListAsync();
        ViewBag.SubUnits = await context.SubUnits.Where(su => su.IsDeleted == false)
            .Include(su => su.Unit).ToListAsync();
        ViewBag.Religions = await context.Religions.Where(r => r.IsDeleted == false).ToListAsync();
        ViewBag.Educations = await context.Educations.Where(ed => ed.IsDeleted == false).ToListAsync();
        ViewBag.Roles = await context.Roles.Where(r => r.Name != "Admin").ToListAsync();
        var employee = await context.Employee
            .Include(usr => usr.Users)
            .FirstOrDefaultAsync(emp => emp.EmployeeId == id);
        if (employee?.Users != null)
        {
            var roles = await userManager.GetRolesAsync(employee.Users);
            return View(new EditEmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                UserId = employee.UserId,
                FullName = employee.Users.FullName,
                PhoneNumber = employee.Users.PhoneNumber,
                Email = employee.Users.Email,
                Nip = employee.Nip.ToString(),
                Nik = employee.Nik,
                Npwp = employee.Npwp,
                Address = employee.Address,
                CompanyId = employee.CompanyId ?? 0,
                DivisionId = employee.DivisionId ?? 0,
                JobTitleId = employee.JobTitleId ?? 0,
                SubUnitId = employee.SubUnitId ?? 0,
                ReligionId = employee.ReligionId ?? 0,
                EducationId = employee.EducationId ?? 0,
                TaxExemptIncomeId = employee.TaxExemptIncomeId ?? 0,
                Gender = employee.Gender,
                JoinDate = employee.JoinDate ?? DateTime.Now,
                RolesId = roles
            });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditEmployeeViewModel model)
    {
        var admin = await userManager.GetUserAsync(User);
        var time = DateTime.UtcNow;
        if (admin == null)
        {
            return RedirectToAction("Logout", "Account");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Divisions = await divisionService.FindAll();
            ViewBag.JobTitle = await jobTitleService.FindAll();
            ViewBag.Companies = await context.Companies.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Taxes = await context.TaxExemptIncomes.Where(t => t.IsDeleted == false).ToListAsync();
            ViewBag.SubUnits = await context.SubUnits.Where(su => su.IsDeleted == false)
                .Include(su => su.Unit).ToListAsync();
            ViewBag.Religions = await context.Religions.Where(r => r.IsDeleted == false).ToListAsync();
            ViewBag.Educations = await context.Educations.Where(ed => ed.IsDeleted == false).ToListAsync();
            ViewBag.Roles = await context.Roles.Where(r => r.Name != "Admin").ToListAsync();
            return View(model);
        }

        var oldemployee = await context.Employee
            .Include(usr => usr.Users)
            .FirstOrDefaultAsync(emp => emp.EmployeeId == id);

        var updateUser = context.Users.FirstOrDefault(x => x.Id == model.UserId);

        var logs = new AppLogModel
        {
            CreatedBy = $"{admin.Id} - {admin.FullName}",
            CreatedAt = time,
            Source = JsonConvert.SerializeObject(new
            {
                Controller = "EmployeeController",
                Action = "Edit",
                Database = "Employee & AspNetUsers"
            }),
            Params = JsonConvert.SerializeObject(new
            {
                oldData = JsonConvert.SerializeObject(new
                {
                    oldemployee.Nip,
                    oldemployee.Nik,
                    oldemployee.Npwp,
                    oldemployee.Address,
                    oldemployee.DivisionId,
                    oldemployee.JobTitleId,
                    oldemployee.ReligionId,
                    oldemployee.EducationId,
                    oldemployee.TaxExemptIncomeId,
                    oldemployee.SubUnitId,
                    oldemployee.CompanyId,
                    updateUser.FullName,
                    updateUser.Email,
                    updateUser.PhoneNumber,
                }),
                newData = JsonConvert.SerializeObject(new
                {
                    model.Nip,
                    model.Nik,
                    model.Npwp,
                    model.Address,
                    model.DivisionId,
                    model.JobTitleId,
                    model.ReligionId,
                    model.EducationId,
                    model.TaxExemptIncomeId,
                    model.SubUnitId,
                    model.CompanyId,
                    model.FullName,
                    model.Email,
                    model.PhoneNumber
                })
            })
        };


        updateUser.FullName = model.FullName;
        updateUser.Email = model.Email;
        updateUser.PhoneNumber = model.PhoneNumber;
        //save
        await context.SaveChangesAsync();

        var roles = await userManager.GetRolesAsync(updateUser);
        await userManager.RemoveFromRolesAsync(updateUser, roles);
        await userManager.AddToRolesAsync(updateUser, model.RolesId);

        var updateEmployee = context.Employee.FirstOrDefault(x => x.EmployeeId == id);
        updateEmployee.Nip = int.Parse(model.Nip);
        updateEmployee.Nik = model.Nik;
        updateEmployee.Npwp = model.Npwp;
        updateEmployee.Address = model.Address;

        updateEmployee.DivisionId = model.DivisionId;
        updateEmployee.JobTitleId = model.JobTitleId;
        updateEmployee.ReligionId = model.ReligionId;
        updateEmployee.EducationId = model.EducationId;
        updateEmployee.TaxExemptIncomeId = model.TaxExemptIncomeId;
        updateEmployee.SubUnitId = model.SubUnitId;
        updateEmployee.CompanyId = model.CompanyId;
        updateEmployee.UpdatedAt = time;
        updateEmployee.UpdatedBy = admin.Id;
        //update employee
        await context.SaveChangesAsync();
        await mongoDbContext.AppLogs.InsertOneAsync(logs);

        TempData["Success"] = $"Employee: {updateUser.FullName} has been updated";
        return RedirectToAction(nameof(Index));
    }

    private string GenerateUsername(string email)
    {
        var username = email.Split('@')[0];
        return username;
    }

    public IActionResult Import()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(ImportEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await ReadEmployeeFromExcelAsync(model.ExcelFile);
        if (user.Count == 0)
        {
            TempData["Errors"] = "No data found";
            return RedirectToAction(nameof(Import));
        }

        TempData["Success"] = "Data has been imported";
        return Ok(user);
    }

    private async Task<List<object>> ReadEmployeeFromExcelAsync(IFormFile excelFile)
    {
        var employees = new List<object>();
        await using var stream = excelFile.OpenReadStream();
        using var reader = ExcelReaderFactory.CreateReader(stream);
        bool isFirstSheet = true;

        do
        {
            if (isFirstSheet)
            {
                reader.Read();

                while (reader.Read())
                {
                    if (reader.IsDBNull(0) || reader.IsDBNull(1) || reader.IsDBNull(2) ||
                        reader.IsDBNull(3) || reader.IsDBNull(4) || reader.IsDBNull(5) ||
                        reader.IsDBNull(6) || reader.IsDBNull(7) || reader.IsDBNull(8))
                    {
                        continue;
                    }

                    var existingUser = await userManager.FindByEmailAsync(reader.GetValue(3).ToString());
                    if (existingUser != null)
                    {
                        continue;
                    }

                    // Buat user baru.
                    var password = reader.GetValue(0).ToString();
                    if (password.Length < 4)
                    {
                        password = $"00{password}";
                    }

                    var newUser = new Users
                    {
                        UserName = reader.GetValue(2).ToString(),
                        Email = reader.GetValue(3).ToString(),
                        FullName = reader.GetValue(1).ToString(),
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = "081234567890"
                    };
                    var createUserResult = await userManager.CreateAsync(newUser, password);
                    if (!createUserResult.Succeeded)
                    {
                        continue;
                    }

                    var asignRole = await userManager.FindByEmailAsync(reader.GetValue(3).ToString());
                    await userManager.AddToRoleAsync(asignRole, "User");
                    var userId = newUser.Id;

                    var employee = new EmployeeModel
                    {
                        UserId = userId,
                        Nip = int.Parse(reader.GetValue(0).ToString()),
                        Nik = "1234567887654321",
                        Npwp = "1234567887654321",
                        SubUnitId = int.Parse(reader.GetValue(5).ToString()),
                        DivisionId = int.Parse(reader.GetValue(6).ToString()),
                        CompanyId = int.Parse(reader.GetValue(7).ToString()),
                        JobTitleId = int.Parse(reader.GetValue(8).ToString()),
                        EducationId = 7,
                        JoinDate = new DateTime(2020, 1, 1),
                        TaxExemptIncomeId = 3,
                        Address = "Jakarta",
                        Gender = reader.GetValue(4).ToString()
                    };

                    await context.Employee.AddAsync(employee);
                    await context.SaveChangesAsync();
                    var employeeResult = new
                    {
                        Nip = reader.GetValue(0),
                        Nama = reader.GetValue(1),
                        Username = reader.GetValue(2),
                        Email = reader.GetValue(3),
                        SubUnit = reader.GetValue(4),
                        Division = reader.GetValue(5),
                        Company = reader.GetValue(6),
                        JobTitle = reader.GetValue(7)
                    };

                    employees.Add(employeeResult);
                }

                isFirstSheet = false;
            }
        } while (reader.NextResult());

        return employees;
    }
}
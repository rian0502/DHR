using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class EmployeeService(AppDbContext context)
{
    //FindAll
    public async Task<IEnumerable<EmployeeModel>> FindAll()
    {
        var results = await context.Employee
            .Include(x => x.Users)
            .Include(x => x.JobTitle)
            .Include(x => x.SubUnit)
            .Include(x => x.Division)
            .ToListAsync();
        return results;
    }
    //FindById
    public async Task<EmployeeModel?> FindById(int id)
    {
        var result = await context.Employee
            .Include(x => x.Users)
            .Include(x => x.JobTitle)
            .Include(x => x.SubUnit)
            .Include(x => x.Division)
            .Include(x => x.TaxExemptIncome)
            .Include(x => x.Education)
            .Include(x => x.Religion)
            .Include(x => x.EmployeeDependents)
            .FirstOrDefaultAsync(x => x.EmployeeId == id);
        return result;
    }
}
using DHR.Helper;
using DHR.Models;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers;

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
            .OrderBy(e => e.Nip)
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
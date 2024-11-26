using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class EmployeeService(AppDBContext context)
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
}
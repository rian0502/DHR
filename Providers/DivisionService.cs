using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class DivisionService(AppDBContext context)
{
    private const string StoreProcedure = "sp_Division"; 
    
    
    //FindAll
    public async Task<IEnumerable<DivisionModel>> FindAll()
    {
        var results = await context.Divisions
            .Include(d => d.SubDepartment)
            .ToListAsync();
        return results;
    }
}
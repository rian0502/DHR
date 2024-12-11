using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers
{
    public class PeriodService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        private readonly string _storeProcedure = "sp_Period";
        //Find All
        public async Task<IEnumerable<PeriodModel>> FindAll()
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'FindAll'").ToListAsync();
        }

        //Find All
        public async Task<IEnumerable<PeriodModel>> AttandancePeriod()
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'AttandancePeriod'").ToListAsync();
        }
        
        
        
        //Find Active Periode
        public async Task<PeriodModel> ActivePeriod()
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'ActivePeriod'").FirstOrDefaultAsync();
        }
    }
}

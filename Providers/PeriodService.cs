using DHR.Helper;
using DHR.Models;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers
{
    public class PeriodService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        private readonly string _storeProcedure = "sp_Period";
        //Find All
        public async Task<IEnumerable<PeriodModel?>> FindAll()
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'FindAll'").ToListAsync();
        }

        //Find All
        public async Task<IEnumerable<PeriodModel?>> AttendancePeriod()
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'AttendancePeriod'").ToListAsync();
        }
        
        //Find ById
        public async Task<PeriodModel> FindById(int id)
        {
            return await _context.Periods.FromSql($"EXECUTE {_storeProcedure} @Action = 'FindById', @Id = {id}").FirstOrDefaultAsync();
        }
    }
}

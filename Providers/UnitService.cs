using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.Unit;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers
{
    public class UnitService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        private readonly string _storeProcedure = "sp_Unit";
        // FindAll
        public async Task<IEnumerable<UnitModel>> FindAll()
        {
            var results = await _context.Units.FromSql($"EXEC {_storeProcedure} @Action = 'FindAll'").ToListAsync();
            return results;
        }

        // FindById
        public async Task<UnitModel> FindById(int id)
        {
            var result = await _context.Units.FromSql($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}").ToListAsync();
            return result.FirstOrDefault();
        }

        // Insert
        public async Task<int> Insert(CreateUnitViewModel unit, string userId, DateTime time)
        {
            var check = await _context.Units.FirstOrDefaultAsync(x => x.UnitCode == unit.UnitCode || x.UnitName == unit.UnitName);
            if (check != null)
            {
                return 3;
            }
            var result = await _context.Database.ExecuteSqlAsync($@"
                      EXEC {_storeProcedure}
                      @Action = 'Insert', 
                      @UnitCode = {unit.UnitCode},
                      @UnitName = {unit.UnitName}, 
                      @CreatedBy = {userId}, 
                      @CreatedAt = {time}, 
                      @UpdatedBy = {userId}, 
                      @UpdatedAt = {time}
            ");
            return result;
        }

        // Update
        public async Task<int> Update(EditUnitViewModel unit, string userId, DateTime time)
        {
            var result = await _context.Database.ExecuteSqlAsync($@"
                      EXEC {_storeProcedure} 
                      @Action = 'Update', 
                      @Id = {unit.UnitId}, 
                      @UnitCode = {unit.UnitCode}, 
                      @UnitName = {unit.UnitName}, 
                      @UpdatedBy = {userId}, 
                      @UpdatedAt = {time}
            ");
            return result;
        }

        // Delete
        public async Task<int> Delete(int id)
        {
            var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Delete', @Id = {id}");
            return result;
        }

    }
}

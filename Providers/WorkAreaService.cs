using Microsoft.EntityFrameworkCore;
using Presensi360.Helper;
using Presensi360.Models;
using Presensi360.ViewModels;

namespace Presensi360.Providers;

public class WorkAreaService(AppDBContext context)
{
    private readonly AppDBContext _context = context;
    private readonly string _storeProcedure = "sp_Location";

    //FindAll
    public async Task<IEnumerable<LocationModel>> FindAll()
    {
        var results = await _context.Locations
            .FromSql($"EXEC {_storeProcedure} @Action = 'FindAll'")
            .ToListAsync();
        return results;
    }
    //FindById
    public async Task<LocationModel> FindById(int id)
    {
        var results = await _context.Locations
            .FromSqlInterpolated($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}")
            .ToListAsync();
        return results.FirstOrDefault();
    }
    //Create
    public async Task<int> Create(CreateWorkAreaViewModel model)
    {
        var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Insert', @Code = {model.LocationCode}, @Name = {model.LocationName}");
        return result;
    }
    //Update
    public async Task<int> Update(EditWorkAreaViewModel model)
    {
        var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Update', @Id = {model.LocationID}, @Code = {model.LocationCode}, @Name = {model.LocationName}");
        return result;
    }

}
using System.Data;
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class DashboardService(string connectionString)
{
    public async Task<List<object>> GetAllowance(string startYear, string endYear, int nip)
    {
        var dashboards = new List<object>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @StartYear, @EndYear, @NIP",
                connection);
        command.Parameters.AddWithValue("@Action", "GetAllowance");
        command.Parameters.AddWithValue("@StartYear", startYear);
        command.Parameters.AddWithValue("@EndYear", endYear);
        command.Parameters.AddWithValue("@NIP", nip);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Period = reader.GetString(reader.GetOrdinal("Period")),
                TotalMealAllowance = reader.GetDouble(reader.GetOrdinal("TotalMealAllowance")),
            };
            dashboards.Add(dashboard);
        }
        return dashboards;
    }
    
    public async Task<List<object>> GetAttendance(string startYear, string endYear, int nip)
    {
        var dashboards = new List<object>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @StartYear, @EndYear, @NIP",
                connection);
        command.Parameters.AddWithValue("@Action", "GetAttendance");
        command.Parameters.AddWithValue("@StartYear", startYear);
        command.Parameters.AddWithValue("@EndYear", endYear);
        command.Parameters.AddWithValue("@NIP", nip);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Period = reader.GetString(reader.GetOrdinal("Period")),
                Entered = reader.GetInt32(reader.GetOrdinal("Entered")),
            };
            dashboards.Add(dashboard);
        }
        return dashboards;
    }
}
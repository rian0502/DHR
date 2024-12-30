using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class DashboardService(string connectionString)
{
    public async Task<List<object>> GetAllowance(string yearPeriod, int nip)
    {
        var dashboards = new List<object>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @YearPeriod, @NIP",
                connection);
        command.Parameters.AddWithValue("@Action", "GetAllowance");
        command.Parameters.AddWithValue("@YearPeriod", yearPeriod);
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

    public async Task<List<object>> GetAttendance(string yearPeriod, int nip)
    {
        var dashboards = new List<object>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @YearPeriod, @NIP", connection);
        command.Parameters.AddWithValue("@Action", "GetAttendance");
        command.Parameters.AddWithValue("@NIP", nip);
        command.Parameters.AddWithValue("@YearPeriod", yearPeriod);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Period = reader.GetString(reader.GetOrdinal("Period")),
                Entered = reader.GetInt32(reader.GetOrdinal("Entered"))
            };
            dashboards.Add(dashboard);
        }

        return dashboards;
    }

    public async Task<List<object>> GetLeaveRequest(string yearPeriod, int nip)
    {
        var dashboards = new List<object>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @YearPeriod, @NIP",
                connection);
        command.Parameters.AddWithValue("@Action", "GetLeaveRequest");
        command.Parameters.AddWithValue("@YearPeriod", yearPeriod);
        command.Parameters.AddWithValue("@NIP", nip);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Period = reader.GetString(reader.GetOrdinal("Period")),
                TimeOff = reader.GetDouble(reader.GetOrdinal("TimeOff")),
            };
            dashboards.Add(dashboard);
        }

        return dashboards;
    }

    public async Task<List<object>> GetMedicalClaim(string yearPeriod, int nip)
    {
        var dashboards = new List<object>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("EXEC [dbo].[GetEmployeeDashboardData] @Action, @YearPeriod, @NIP",
                connection);
        command.Parameters.AddWithValue("@Action", "GetMedicalClaim");
        command.Parameters.AddWithValue("@YearPeriod", yearPeriod);
        command.Parameters.AddWithValue("@NIP", nip);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Period = reader.GetString(reader.GetOrdinal("Period")),
                Claim = reader.GetInt32(reader.GetOrdinal("Claim")),
            };
            dashboards.Add(dashboard);
        }

        return dashboards;
    }

    public async Task<List<object>> GetYearPeriod()
    {
        var dashboards = new List<object>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand("SELECT * FROM PeriodsView", connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dashboard = new
            {
                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                StartPeriod = reader.GetDateTime(reader.GetOrdinal("StartPeriod")),
                EndPeriod = reader.GetDateTime(reader.GetOrdinal("EndPeriod")),
                IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")),
            };
            dashboards.Add(dashboard);
        }

        return dashboards;
    }
}
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class YearPeriodService(string connectionString)
{
    public async Task<List<object>> GetYearPeriod()
    {
        var yearPeriods = new List<object>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "SELECT TOP (1000) [Year] ,[StartPeriod] ,[EndPeriod] ,[IsActive] FROM [PeriodsView]",
            connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var newYearPeriod = new
            {
                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                StartPeriod = reader.GetDateTime(reader.GetOrdinal("StartPeriod")),
                EndPeriod = reader.GetDateTime(reader.GetOrdinal("EndPeriod")),
                IsActive = reader.GetInt32(reader.GetOrdinal("IsActive"))
            };
            yearPeriods.Add(newYearPeriod);
        }
        return yearPeriods;
    }
}
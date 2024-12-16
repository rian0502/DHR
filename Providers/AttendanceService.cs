using Microsoft.Data.SqlClient;
using DHR.Models;

namespace DHR.Providers;

public class AttendanceService(string connectionString)
{
    public async Task<List<AttendanceModel>> GetAttendance(int nip, int periodId)
    {
        var attendances = new List<AttendanceModel>();
  
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("EXEC [dbo].[GetAttendance] @NIP, @PeriodId", connection);
        command.Parameters.AddWithValue("@NIP", nip);
        command.Parameters.AddWithValue("@PeriodId", periodId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var attendance = new AttendanceModel
            {
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                Day = reader.GetByte(reader.GetOrdinal("Day")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                CheckIn = reader.IsDBNull(reader.GetOrdinal("CheckIn"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("CheckIn")),
                CheckOut = reader.IsDBNull(reader.GetOrdinal("CheckOut"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("CheckOut")),
                Late = reader.IsDBNull(reader.GetOrdinal("Late")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Late")),
                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                MealAllowance = reader.IsDBNull(reader.GetOrdinal("MealAllowance"))
                    ? null
                    : reader.GetInt16(reader.GetOrdinal("MealAllowance")),
                BenefitAmount = reader.IsDBNull(reader.GetOrdinal("BenefitAmount"))
                    ? null
                    : reader.GetDouble(reader.GetOrdinal("BenefitAmount"))
            };
            attendances.Add(attendance);
        }
        await connection.CloseAsync();
        Console.WriteLine($"EXEC [dbo].[GetAttendance] @NIP = {nip}, @PeriodId = {periodId}");
        return attendances;
    }
}
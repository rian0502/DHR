using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Presensi360.Helper;
using Presensi360.Models;
using System.Configuration;
using System.Data;

namespace Presensi360.Providers
{
    public class AttendanceService
    {
        private readonly string _connectionString;
        public AttendanceService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<AttendanceModel>> GetAttendance(int employeeId, int periodId)
        {
            var attendances = new List<AttendanceModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("EXEC [dbo].[GetAttendance] @EmplID, @PeriodId", connection))
                {
                    command.Parameters.AddWithValue("@EmplID", employeeId);
                    command.Parameters.AddWithValue("@PeriodId", periodId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var attendance = new AttendanceModel
                            {
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                Day = reader.GetByte(reader.GetOrdinal("Day")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                                CheckIn = reader.IsDBNull(reader.GetOrdinal("CheckIn")) ? null : reader.GetDecimal(reader.GetOrdinal("CheckIn")),
                                CheckOut = reader.IsDBNull(reader.GetOrdinal("CheckOut")) ? null : reader.GetDecimal(reader.GetOrdinal("CheckOut")),
                                Late = reader.IsDBNull(reader.GetOrdinal("Late")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Late")),
                                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                                MealAllowance = reader.IsDBNull(reader.GetOrdinal("MealAllowance")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("MealAllowance"))
                            };
                            attendances.Add(attendance);
                        }
                    }
                }
            }

            return attendances;
        }
    }
}

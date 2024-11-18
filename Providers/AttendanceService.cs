using Microsoft.Data.SqlClient;
using Presensi360.Models;
using System.Configuration;
using System.Data;

namespace Presensi360.Providers
{
    public class AttendanceService
    {
        private readonly string _connectionString;

        public AttendanceService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<AttendanceModel> GetAttendance(int emplId, int periodId)
        {
            var attendanceList = new List<AttendanceModel>();

            // Gunakan _connectionString yang sudah diinisialisasi
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("GetAttendance", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@EmplID", SqlDbType.Int) { Value = emplId });
                    command.Parameters.Add(new SqlParameter("@PeriodId", SqlDbType.Int) { Value = periodId });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var attendance = new AttendanceModel
                            {
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                Day = reader.GetInt32(reader.GetOrdinal("Day")),
                                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Date")),
                                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                                CheckIn = reader.IsDBNull(reader.GetOrdinal("CheckIn")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CheckIn")),
                                CheckOut = reader.IsDBNull(reader.GetOrdinal("CheckOut")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CheckOut")),
                                Late = reader.IsDBNull(reader.GetOrdinal("Late")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Late")),
                                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                                MealAllowance = reader.IsDBNull(reader.GetOrdinal("MealAllowance")) ? null : reader.GetInt32(reader.GetOrdinal("MealAllowance"))
                            };

                            attendanceList.Add(attendance);
                        }
                    }
                }
            }

            return attendanceList;
        }
    }
}

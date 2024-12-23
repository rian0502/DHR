using System.Data;
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class LeaveRequestService(string connectionString)
{
    public async Task<int> InsertBatchLeaveRequestsAsync(IEnumerable<object> requests)
    {
        var result = 0;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var request in requests)
            {
                await using var command = new SqlCommand(
                    "EXEC [dbo].[sp_BatchInsert_LeaveRequests] @NIP, @LeaveDate, @LeaveDays, @LeaveType, @LeaveReason, @CreatedBy, @CreatedAt",
                    connection, transaction);

                command.Parameters.Add(new SqlParameter("@NIP", SqlDbType.Int)
                    { Value = GetPropertyValue(request, "Nip") });
                
                command.Parameters.Add(new SqlParameter("@LeaveDate", SqlDbType.Date)
                    { Value = GetDateTimePropertyValue(request, "LeaveDate") });
                
                command.Parameters.Add(new SqlParameter("@LeaveDays", SqlDbType.Float)
                    { Value = (object)GetPropertyValue(request, "LeaveDays") ?? DBNull.Value });
                
                command.Parameters.Add(new SqlParameter("@LeaveType", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(request, "LeaveType") });
                
                command.Parameters.Add(new SqlParameter("@LeaveReason", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(request, "LeaveReason") });
                
                command.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar, 255)
                    { Value = GetPropertyValue(request, "CreatedBy") });
                
                command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime)
                    { Value = GetPropertyValue(request, "CreatedAt") });

                result += await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return result;
        }
        catch (SqlException ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Database error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"An error occurred: {ex.Message}", ex);
        }
    }

    private static object GetPropertyValue(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property?.GetValue(obj) ?? DBNull.Value;
    }

    private static object GetDateTimePropertyValue(object obj, string propertyName)
    {
        var value = GetPropertyValue(obj, propertyName);
        return value is DateOnly dateOnly ? dateOnly.ToDateTime(TimeOnly.MinValue) : DBNull.Value;
    }
}
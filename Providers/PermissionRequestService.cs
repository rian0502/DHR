using System.Data;
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class PermissionRequestService
{
    private readonly string _connectionString;

    public PermissionRequestService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertBatchPermissionRequestsAsync(IEnumerable<object> requests)
    {
        var result = 0;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var request in requests)
            {
                await using var command = new SqlCommand(
                    "EXEC [dbo].[sp_BatchInsert_PermissionRequests] @EmployeePermissionRequestCode, " +
                    "@PermissionDate, @PermissionDays, @PermissionType, @PermissionReason, @EmployeeId, " +
                    "@PersonnelRemarks, @CreatedBy, @CreatedAt",
                    connection, transaction);

                command.Parameters.Add(new SqlParameter("@EmployeePermissionRequestCode", SqlDbType.VarChar, 100)
                    { Value = GetPropertyValue(request, "EmployeePermissionRequestCode") });
                command.Parameters.Add(new SqlParameter("@PermissionDate", SqlDbType.Int)
                    { Value = GetPropertyValue(request, "EmployeeId") });
                command.Parameters.Add(new SqlParameter("@PermissionDays", SqlDbType.Float)
                    { Value = GetDateTimePropertyValue(request, "PermissionDays") });
                command.Parameters.Add(new SqlParameter("@PermissionType", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(request, "RequestType") });
                command.Parameters.Add(new SqlParameter("@PermissionReason", SqlDbType.VarChar, 255)
                    { Value = GetPropertyValue(request, "PermissionReason") });
                command.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int)
                    { Value = GetPropertyValue(request, "EmployeeId") });
                command.Parameters.Add(new SqlParameter("@PersonnelRemarks", SqlDbType.VarChar, 255)
                    { Value = GetPropertyValue(request, "PersonnelRemarks") });
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
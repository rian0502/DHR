using System.Data;
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class WorkEntryService(string connectionString)
{
    public async Task<int> InsertBatchWorkEntryAsync(IEnumerable<object> requests)
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
                    @"EXEC [dbo].[sp_BatchInsert_WorkEntry]
                      @NIP = @NIP,
                      @EmployeeWorkEntryCode = @EmployeeWorkEntryCode,
                      @WorkDate = @WorkDate,
                      @WorkStartTime = @WorkStartTime,
                      @WorkEndTime = @WorkEndTime,
                      @WorkReason = @WorkReason,
                      @PersonnelRemark = @PersonnelRemark,
                      @CreatedBy = @CreatedBy,
                      @CreatedAt = @CreatedAt",
                    connection, transaction);

                command.Parameters.Add(new SqlParameter("@NIP", SqlDbType.Int)
                {
                    Value = GetPropertyValue(request, "NIP")
                });

                command.Parameters.Add(new SqlParameter("@EmployeeWorkEntryCode", SqlDbType.VarChar, 100)
                {
                    Value = GetPropertyValue(request, "EmployeeWorkEntryCode")
                });

                command.Parameters.Add(new SqlParameter("@WorkDate", SqlDbType.Date)
                {
                    Value = GetDateOnlyPropertyValue(request, "WorkDate")
                });

                command.Parameters.Add(new SqlParameter("@WorkStartTime", SqlDbType.Time)
                {
                    Value = GetTimePropertyValue(request, "WorkStartTime")
                });

                command.Parameters.Add(new SqlParameter("@WorkEndTime", SqlDbType.Time)
                {
                    Value = GetTimePropertyValue(request, "WorkEndTime")
                });

                command.Parameters.Add(new SqlParameter("@WorkReason", SqlDbType.VarChar, 255)
                {
                    Value = GetPropertyValue(request, "WorkReason")
                });

                command.Parameters.Add(new SqlParameter("@PersonnelRemark", SqlDbType.VarChar, 255)
                {
                    Value = GetPropertyValue(request, "PersonnelRemark")
                });

                command.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar, 255)
                {
                    Value = GetPropertyValue(request, "CreatedBy")
                });

                command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime)
                {
                    Value = GetPropertyValue(request, "CreatedAt")
                });
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

    private static object GetDateOnlyPropertyValue(object obj, string propertyName)
    {
        var value = GetPropertyValue(obj, propertyName);
        return value is DateOnly dateOnly ? dateOnly.ToDateTime(TimeOnly.MinValue).Date : DBNull.Value;
    }

    private static object GetTimePropertyValue(object obj, string propertyName)
    {
        var value = GetPropertyValue(obj, propertyName);
        return value is TimeOnly timeOnly ? timeOnly.ToTimeSpan() :
               value is TimeSpan timeSpan ? timeSpan :
               DBNull.Value;
    }
}

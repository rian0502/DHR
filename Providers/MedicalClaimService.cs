using System.Data;
using DHR.Models;
using Microsoft.Data.SqlClient;

namespace DHR.Providers;

public class MedicalClaimService
{
    private readonly string _connectionString;

    public MedicalClaimService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertBatchMedicalClaimsAsync(IEnumerable<object> claims)
    {
        var result = 0;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var claim in claims)
            {
                await using var command = new SqlCommand(
                    "EXEC [dbo].[sp_BatchInsert_MedicalClaims] @Period, @NIP, @ClaimDate, @ClaimCategory, @Diagnosis, @ClaimStatus, @PaymentPeriod, @CreatedBy, @CreatedAt",
                    connection, transaction);

                command.Parameters.Add(new SqlParameter("@Period", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(claim, "EmployeeName") });
                command.Parameters.Add(new SqlParameter("@NIP", SqlDbType.Int)
                    { Value = GetPropertyValue(claim, "EmployeeId") });
                command.Parameters.Add(new SqlParameter("@ClaimDate", SqlDbType.Date)
                    { Value = GetDateTimePropertyValue(claim, "ClaimDate") });
                command.Parameters.Add(new SqlParameter("@ClaimCategory", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(claim, "Description") });
                command.Parameters.Add(new SqlParameter("@Diagnosis", SqlDbType.VarChar, 255)
                    { Value = GetPropertyValue(claim, "Diagnosis") });
                command.Parameters.Add(new SqlParameter("@ClaimStatus", SqlDbType.VarChar, 50)
                    { Value = GetPropertyValue(claim, "TreatmentType") });
                command.Parameters.Add(new SqlParameter("@PaymentPeriod", SqlDbType.Date)
                    { Value = GetDateTimePropertyValue(claim, "PaymentPeriod") });
                command.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar, 255)
                    { Value = GetPropertyValue(claim, "CreatedBy") });
                command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime)
                    { Value = GetPropertyValue(claim, "CreatedAt") });

                result += await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
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

        return result;
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
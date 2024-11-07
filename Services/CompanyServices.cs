using Microsoft.Data.SqlClient;
using Presensi360.Models;

namespace Presensi360.Services
{
    public class CompanyServices
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CompanyServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<CompanyModel>> FindAll()
        {
            var Companies = new List<CompanyModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.MSCompanies @Action = @ActionParam";
                    command.Parameters.AddWithValue("@ActionParam", "findAll");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Companies.Add(new CompanyModel
                            {
                                CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                CompanyCode = reader.GetString(reader.GetOrdinal("CompanyKode")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                LocationID = reader.IsDBNull(reader.GetOrdinal("LokasiID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LokasiID")),
                                LocationModel = new LocationModel
                                {
                                    LocationID = reader.GetInt32(reader.GetOrdinal("LokasiID")),
                                    LocationCode = reader.GetString(reader.GetOrdinal("LokasiKode")),
                                    LocationName = reader.GetString(reader.GetOrdinal("LokasiName")),
                                    LocationType = reader.GetString(reader.GetOrdinal("LokasiType"))
                                }
                            });
                        }
                    }
                }
            }

            return Companies;
        }
        public async Task<CompanyModel> FindById(int id)
        {
            CompanyModel company = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    // Set the command to execute the stored procedure MSCompanies with 'findById' action
                    command.CommandText = "EXEC dbo.MSCompanies @Action = @ActionParam, @Id = @IdParam";
                    command.Parameters.AddWithValue("@ActionParam", "findById");
                    command.Parameters.AddWithValue("@IdParam", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            company = new CompanyModel
                            {
                                CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                CompanyCode = reader.GetString(reader.GetOrdinal("CompanyKode")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                LocationID = reader.IsDBNull(reader.GetOrdinal("LokasiID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LokasiID")),
                                LocationModel = new LocationModel
                                {
                                    LocationID = reader.GetInt32(reader.GetOrdinal("LokasiID")),
                                    LocationCode = reader.GetString(reader.GetOrdinal("LokasiKode")),
                                    LocationName = reader.GetString(reader.GetOrdinal("LokasiName")),
                                    LocationType = reader.GetString(reader.GetOrdinal("LokasiType"))
                                }
                            };
                        }
                    }
                }
            }

            return company;
        }

    }
}

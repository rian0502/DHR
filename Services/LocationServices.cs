using Microsoft.Data.SqlClient;
using Presensi360.Models;

namespace Presensi360.Services
{
    public class LocationServices
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public LocationServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<LocationModel>> FindAll()
        {
            var Locations = new List<LocationModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.MSLocations @Action = @ActionParam";
                    command.Parameters.AddWithValue("@ActionParam", "findAll");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Locations.Add(new LocationModel
                            {
                                LocationID = reader.GetInt32(reader.GetOrdinal("LokasiID")),
                                LocationCode = reader.GetString(reader.GetOrdinal("LokasiKode")),
                                LocationName = reader.GetString(reader.GetOrdinal("LokasiName")),
                                LocationType = reader.GetString(reader.GetOrdinal("LokasiType"))
                            });
                        }
                    }
                }
            }

            return Locations;
        }
    }
}

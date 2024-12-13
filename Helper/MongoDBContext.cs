using DHR.Models;
using MongoDB.Driver;

namespace DHR.Helper
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoDBSettings");
            var client = new MongoClient(mongoSettings.GetValue<string>("ConnectionString"));
            _database = client.GetDatabase(mongoSettings.GetValue<string>("DatabaseName"));
        }
        public IMongoCollection<AppLogModel> AppLogs => _database.GetCollection<AppLogModel>("LogApp");
    }
}

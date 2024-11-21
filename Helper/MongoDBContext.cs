using DAHAR.Models;
using MongoDB.Driver;

namespace DAHAR.Helper
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoDBSettings");
            var client = new MongoClient(mongoSettings.GetValue<string>("ConnectionString"));
            _database = client.GetDatabase(mongoSettings.GetValue<string>("DatabaseName"));
        }
        public IMongoCollection<AppLogModel> AppLogs => _database.GetCollection<AppLogModel>("LogApp");
    }
}

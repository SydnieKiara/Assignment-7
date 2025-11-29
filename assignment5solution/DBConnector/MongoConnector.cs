using MongoDB.Bson;
using MongoDB.Driver;

namespace DBConnector
{
    public class MongoConnector : IDBConnector
    {
        private readonly string _connectionString;

        public MongoConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                var client = new MongoClient(_connectionString);
                var database = client.GetDatabase("admin");
                var command = new BsonDocument("ping", 1);
                await database.RunCommandAsync<BsonDocument>(command);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

using Npgsql;

namespace DBConnector
{
    public class PostgresConnector : IDBConnector
    {
        private readonly NpgsqlConnection _connection;

        public PostgresConnector(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                await _connection.OpenAsync();

                await using var cmd = new NpgsqlCommand("SELECT 1", _connection);
                var result = await cmd.ExecuteScalarAsync();

                return result != null && (int)result == 1;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}

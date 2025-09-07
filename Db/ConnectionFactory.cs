using Microsoft.Data.SqlClient;

namespace fotosmigracion.Db
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString) => _connectionString = connectionString;

        public SqlConnection CreateConnection() => new(_connectionString);
    }
}

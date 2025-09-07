using Microsoft.Data.SqlClient;

namespace fotosmigracion.Db
{
    public interface IConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}

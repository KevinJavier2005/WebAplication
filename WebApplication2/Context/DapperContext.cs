using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication2.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Falta la cadena de conexión 'DefaultConnection'.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

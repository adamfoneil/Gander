using System.Data;
using System.Data.SqlClient;

namespace Gander.Library.Environments
{
    /// <summary>
    /// SqlServer-specific test environment
    /// </summary>
    public class SqlEnvironment : Environment
    {
        public override IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
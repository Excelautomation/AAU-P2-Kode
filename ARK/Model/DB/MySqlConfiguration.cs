using System.Data.Entity;
using System.Data.Entity.Migrations.Infrastructure;

namespace ARK.Model.DB
{
    public class MySqlConfiguration : DbConfiguration
    {
        public MySqlConfiguration()
        {
            SetHistoryContext(
                "MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }
    }
}
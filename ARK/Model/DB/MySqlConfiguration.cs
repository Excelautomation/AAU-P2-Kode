using System.Data.Entity;

namespace ARK.Model.DB
{
    public class MySqlConfiguration : DbConfiguration
    {
        #region Constructors and Destructors

        public MySqlConfiguration()
        {
            this.SetHistoryContext("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }

        #endregion
    }
}
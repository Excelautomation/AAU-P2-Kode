using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace ARK.Model.DB
{
    public class MySqlHistoryContext : HistoryContext
    {
        #region Constructors and Destructors

        public MySqlHistoryContext(DbConnection existingConnection, string defaultSchema)
            : base(existingConnection, defaultSchema)
        {
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
        }

        #endregion
    }
}
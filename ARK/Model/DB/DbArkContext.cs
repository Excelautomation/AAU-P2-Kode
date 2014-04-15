using System.Data.Entity;

namespace ARK.Model.DB
{
    public class DbArkContext : DbContext
    {
        public DbArkContext() : base("MikkelsNoobDB")
        {
            //Database.SetInitializer<DbArkContext>(new DropCreateDatabaseAlways<DbArkContext>());
            Database.SetInitializer(new MySqlInitializer());
        }

        public DbSet<FtpInfo> FtpInfo { get; set; }
        public DbSet<DamageType> DamageType { get; set; }
        public DbSet<Boat> Boat { get; set; }
        public DbSet<LongTripForm> LongTripForm { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<DamageDescription> DamageDescription { get; set; }
        public DbSet<DamageForm> DamageForm { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<GetSMS> GetSMS { get; set; }
        public DbSet<SMS> SMS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Boat)
                .WithMany()
                .HasForeignKey(t => t.Boat_BoatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(d => d.DamagedBoat)
                .WithRequiredDependent()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
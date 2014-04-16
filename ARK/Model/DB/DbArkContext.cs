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
        public DbSet<LongDistanceForm> LongTripForm { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<DamageDescription> DamageDescription { get; set; }
        public DbSet<DamageForm> DamageForm { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<GetSMS> GetSMS { get; set; }
        public DbSet<SMS> SMS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LongDistanceForm>()
                .HasRequired(ldf => ldf.Boat)
                .WithMany(b => b.LongDistanceForms)
                //.HasForeignKey(ldf => ldf.BoatId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<LongDistanceForm>()
                .HasMany(ldf => ldf.Members)
                .WithMany(m => m.LongDistanceForms);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Boat)
                .WithMany(b => b.Trips)
                //.HasForeignKey(t => t.BoatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Members)
                .WithMany(m => m.Trips);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(df => df.Member)
                .WithMany(m => m.DamageForms)
                //.HasForeignKey(df => df.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(df => df.Boat)
                .WithMany(b => b.DamageForms)
                //.HasForeignKey(df => df.BoatId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(df => df.DamageDescription)
                .WithOptional()
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
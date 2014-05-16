using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

using ARK.HelperFunctions.SMSGateway;

namespace ARK.Model.DB
{
    public class DbArkContext : DbContext
    {
        #region Static Fields

        private static DbArkContext _dbContext;

        #endregion

        #region Constructors and Destructors

        public DbArkContext()
            : base("DefaultDB")
        {
            // Database.SetInitializer<DbArkContext>(new DropCreateDatabaseAlways<DbArkContext>());
            Database.SetInitializer(new MySqlInitializer());
            this.Database.Log = s => Debug.WriteLine("DBContext: " + s);
        }

        #endregion

        #region Public Properties

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Boat> Boat { get; set; }

        public DbSet<DamageForm> DamageForm { get; set; }

        public DbSet<DamageType> DamageType { get; set; }

        public DbSet<FTPInfo> FtpInfo { get; set; }

        public DbSet<GetSMS> GetSMS { get; set; }

        public DbSet<LongTripForm> LongTripForm { get; set; }

        public DbSet<Member> Member { get; set; }

        public DbSet<SMS> SMS { get; set; }

        public DbSet<Season> Season { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<StandardTrip> StandardTrip { get; set; }

        public DbSet<Trip> Trip { get; set; }

        public DbSet<TripWarningSms> TripWarningSms { get; set; }

        #endregion

        #region Public Methods and Operators

        public static DbArkContext GetDbContext()
        {
            return _dbContext ?? (_dbContext = new DbArkContext());
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            // TODO Fjern denne
            Debug.WriteLine("Disposing DBContext");
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boat>()
                .Ignore(b => b.Usable)
                .Ignore(b => b.Damaged)
                .Ignore(b => b.GetActiveTrip)
                .Ignore(b => b.BoatOut)
                .Ignore(b => b.KilometersSailed)
                .Ignore(b => b.TripsSailed)
                .Ignore(b => b.LongDistanceTripsSailed);

            modelBuilder.Entity<Boat>().Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Trip>().Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Member>().Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<LongTripForm>()
                .HasOptional(ltf => ltf.Boat)
                .WithMany(b => b.LongTripForms)
                .HasForeignKey(ltf => ltf.BoatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LongTripForm>()
                .HasRequired(ltf => ltf.ResponsibleMember)
                .WithMany()
                .HasForeignKey(ltf => ltf.ResponsibleMemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LongTripForm>().HasMany(ldf => ldf.Members).WithMany(m => m.LongDistanceForms);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Boat)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BoatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>().HasMany(t => t.Members).WithMany(m => m.Trips);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(df => df.RegisteringMember)
                .WithMany(m => m.DamageForms)
                .HasForeignKey(df => df.RegisteringMemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DamageForm>()
                .HasRequired(df => df.Boat)
                .WithMany(b => b.DamageForms)
                .HasForeignKey(df => df.BoatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Admin>().HasKey(a => a.Username);

            modelBuilder.Entity<Admin>().HasRequired(a => a.Member).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<TripWarningSms>().HasRequired(tws => tws.Trip).WithOptional().WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
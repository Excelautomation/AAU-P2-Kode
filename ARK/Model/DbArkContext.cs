using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;


namespace ARK.Model
{
    class DbArkContext : DbContext
    {
        public DbArkContext() : base("Server=nobs.mclc.dk;Database=ark;Uid=ark;Pwd=bqLWb6nGTPNVRqRb;") { }

        public DbSet<FtpInfo> FTPInfo { get; set; }
        public DbSet<DamageType> DamageType { get; set; }
        public DbSet<Boat> Baad { get; set; }
        public DbSet<LongTripForm> Langtursblanket { get; set; }
        public DbSet<Member> Medlem { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<DamageDescription> SkadeBeskrivelse { get; set; }
        public DbSet<DamageForm> Skadesblanket { get; set; }
        public DbSet<Trip> Tur { get; set; }
    }
}

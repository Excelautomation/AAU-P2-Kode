using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ARK.Model
{
    public class DbArkContext : DbContext
    {
        public DbArkContext() : base("MikkelsNoobDB") { }

        public DbSet<FtpInfo> FtpInfo { get; set; }
        public DbSet<DamageType> DamageType { get; set; }
        public DbSet<Boat> Boat { get; set; }
        public DbSet<LongTripForm> LongTripForm { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<DamageDescription> DamageDescription { get; set; }
        public DbSet<DamageForm> DamageForm { get; set; }
        public DbSet<Trip> Trip { get; set; }
    }
}

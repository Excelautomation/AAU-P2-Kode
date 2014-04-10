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
        public DbSet<Baad> Baad { get; set; }
        public DbSet<Langtursblanket> Langtursblanket { get; set; }
        public DbSet<Medlem> Medlem { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<SkadeBeskrivelse> SkadeBeskrivelse { get; set; }
        public DbSet<Skadesblanket> Skadesblanket { get; set; }
        public DbSet<Tur> Tur { get; set; }
    }
}

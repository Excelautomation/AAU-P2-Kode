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
        DbSet<FtpInfo> FTPInfo { get; set; }
        DbSet<DamageType> DamageType { get; set; }
    }
}

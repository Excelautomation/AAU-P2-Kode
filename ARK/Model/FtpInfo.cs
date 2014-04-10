using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
     public class FtpInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}

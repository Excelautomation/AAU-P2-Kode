using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Skadesblanket
    {
        public DateTime Date { get; set; }
        public string ReportedBy { get; set; }
        public int ReportedByNumber { get; set; }
        public Baad DamagedBoat { get; set; }
        public SkadeBeskrivelse Damage { get; set; }
    }
}

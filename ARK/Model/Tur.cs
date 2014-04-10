using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.Model
{
    public class Tur
    {
        public Tur(XMLTure.datarootTur tur)
        {

        }

        public int ID { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
        public bool LongTrip { get; set; }
        public int BoatID { get; set; }
        public List<Medlem> MembersOnTrip;
    }
}

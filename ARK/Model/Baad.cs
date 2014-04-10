using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.Model
{
    public class Baad
    {
        public Baad(XML.XMLBåde.datarootBådeSpecifik bådXML)
        {

        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberofSeats { get; set; }
        public bool Aktive { get; set; }
        public int BoatType { get; set; }
        public bool Usable { get; set; }
        public int SpecifikBoatType { get; set; }
        public bool LongTripBoat { get; set; }   
    }
}

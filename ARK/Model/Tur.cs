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
        public int MemberID_1 { get; set; }
        public int MemberID_2 { get; set; }
        public int MemberID_3 { get; set; }
        public int MemberID_4 { get; set; }
        public int MemberID_5 { get; set; }
        public int MemberID_6 { get; set; }
        public int MemberID_7 { get; set; }
        public int MemberID_8 { get; set; }
        public int MemberID_9 { get; set; }
    }
}

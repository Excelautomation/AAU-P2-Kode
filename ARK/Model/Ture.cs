using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Ture : Search.Searchable<Ture>
    {
        public int ID { get; set; }
        public int Kilometer { get; set; }
        public DateTime Dato { get; set; }
        public bool Langtur { get; set; }
        public int BådID { get; set; }
        public int MedlemsID_1 { get; set; }
        public int MedlemsID_2 { get; set; }
        public int MedlemsID_3 { get; set; }
        public int MedlemsID_4 { get; set; }
        public int MedlemsID_5 { get; set; }
        public int MedlemsID_6 { get; set; }
        public int MedlemsID_7 { get; set; }
        public int MedlemsID_8 { get; set; }
        public int MedlemsID_9 { get; set; }

        public override Ture getTarget()
        {
            return this;
        }
    }
}

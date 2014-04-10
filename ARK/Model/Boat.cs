using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.Model
{
    public class Boat
    {
        public Boat()
        {
        }
        public Boat(XML.XMLBoats.datarootBådeSpecifik boatXML)
        {
            this.ID = boatXML.ID;
            this.Name = boatXML.Navn;
            this.NumberofSeats = boatXML.AntalPladser;
            this.Aktive = boatXML.Aktiv == 1 ? true : false;
            this.SpecificBoatType = (BoatType)SpecificBoatType;
            this.Usable = boatXML.Roforbud == 1 ? true : false;
            this.LongTripBoat = boatXML.LangtursBåd == 1 ? true : false;

        }

        public enum BoatType
        {
            None = 0,
            Inrigger = 1,
            Outrigger = 2,
            Kajak = 3,
            Gig = 4,
            Ergometer = 5,
            UNKOWN = 6
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberofSeats { get; set; }
        public bool Aktive { get; set; }
        public BoatType SpecificBoatType { get; set; }
        public bool Usable { get; set; }
        public bool LongTripBoat { get; set; }   
    }
}

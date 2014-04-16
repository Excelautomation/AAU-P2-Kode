using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;
using System.ComponentModel.DataAnnotations;

namespace ARK.Model
{
    public class Boat
    {
        public Boat()
        {
        }

        public Boat(XML.XMLBoats.datarootBådeSpecifik boatXML)
        {
            this.Id = boatXML.ID;
            this.Name = boatXML.Navn;
            this.NumberofSeats = boatXML.AntalPladser;
            this.Active = boatXML.Aktiv == 1;
            this.SpecificBoatType = (BoatType)SpecificBoatType;
            this.Usable = boatXML.Roforbud == 1;
            this.LongTripBoat = boatXML.LangtursBåd == 1;
            this.DamageForms = new List<DamageForm>();
            this.LongDistanceForms = new LinkedList<LongDistanceForm>();
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

        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberofSeats { get; set; }
        public bool Active { get; set; }
        public BoatType SpecificBoatType { get; set; }
        public bool Usable { get; set; }
        public bool LongTripBoat { get; set; }

        public virtual ICollection<DamageForm> DamageForms { get; set; }
        public virtual ICollection<LongDistanceForm> LongDistanceForms { get; set; }
    }
}

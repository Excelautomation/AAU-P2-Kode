using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.Model
{
    public class Boat
    {
        public enum BoatType
        {
            None = 0,
            Inrigger = 1,
            Outrigger = 2,
            Kajak = 3,
            Gig = 4,
            Ergometer = 5,
            Ukendt = 6
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberofSeats { get; set; }
        public bool Active { get; set; }
        public BoatType SpecificBoatType { get; set; }
        public bool Usable 
        {
            get
            {
                return this.Active && this.DamageForms != null && this.DamageForms.Any(x => !x.Functional && !x.Closed);
            }
        }
        public bool LongTripBoat { get; set; }
        public bool BoatOut { get; set; }

        //Navigation properties
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<DamageForm> DamageForms { get; set; }
        public virtual ICollection<LongDistanceForm> LongDistanceForms { get; set; }
    }
}

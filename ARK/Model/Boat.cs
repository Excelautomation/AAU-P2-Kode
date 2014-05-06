using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.Model
{
    public class Boat : IEquatable<Boat>
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
        public int NumberofSeats { get; set; }          // The number of rowers in the boat without the Deckofficer
        public bool HaveCox { get; set; }       // states if the boat have seating for cox
        public bool Active { get; set; }                // states if the boat is not retired/dead
        public BoatType SpecificBoatType { get; set; }  
        public bool LongTripBoat { get; set; }          // states if the boad is accepted for long tips

        //Navigation properties
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<DamageForm> DamageForms { get; set; }
        public virtual ICollection<LongDistanceForm> LongDistanceForms { get; set; }

        //Not mapped properties
        public bool Usable                              // states if the boat is in a usable condition
        {
            get { return Active && DamageForms != null && !DamageForms.Any(x => !x.Functional && !x.Closed); }
        }

        public bool Damaged                             // Damaged or not
        {
            get { return DamageForms != null && DamageForms.Count != 0; }
        }

        public Trip GetActiveTrip
        {
            get { return Trips.FirstOrDefault(x => x.TripEndedTime == default(DateTime)); }
        }

        public bool BoatOut
        {
            get { return GetActiveTrip != default(Trip); }
        }

        //Interfaces
        public bool Equals(Boat other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Boat) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Boat left, Boat right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Boat left, Boat right)
        {
            return !Equals(left, right);
        }
    }
}
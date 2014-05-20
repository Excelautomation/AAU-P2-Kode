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

        // states if the boat have seating for cox
        public bool Active { get; set; }

        public bool BoatOut
        {
            get
            {
                return GetActiveTrip != default(Trip);
            }
        }

        // states if the boat is not retired/dead
        public virtual ICollection<DamageForm> DamageForms { get; set; }

        public bool Damaged
        {
            // Damaged or not
            get
            {
                return DamageForms != null && DamageForms.Count != 0;
            }
        }

        public Trip GetActiveTrip
        {
            get
            {
                return Trips.FirstOrDefault(trip => trip.TripEndedTime == null);
            }
        }

        public bool HaveCox { get; set; }

        public int Id { get; set; }

        public string InformationString { get; set; }

        public double KilometersSailed
        {
            get
            {
                return Trips.Sum(t => t.Distance);
            }
        }

        public int LongDistanceTripsSailed
        {
            get
            {
                return Trips.Count(t => t.LongTrip);
            }
        }

        public bool LongTripBoat { get; set; }

        public virtual ICollection<LongTripForm> LongTripForms { get; set; }

        public string Name { get; set; }

        public int NewPrice { get; set; }

        public int NumberofSeats { get; set; }

        public BoatType SpecificBoatType { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public int TripsSailed
        {
            get
            {
                return Trips.Count;
            }
        }

        public bool Usable
        {
            // states if the boat is in a usable condition
            get
            {
                return DamageForms != null && !DamageForms.Any(x => !x.Functional && !x.Closed);
            }
        }

        public int Year { get; set; }

        public bool Equals(Boat other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public static BoatType[] GetBoatTypes()
        {
            return new[] { BoatType.Inrigger, BoatType.Outrigger, BoatType.Kajak, BoatType.Gig, BoatType.Ergometer };
        }

        public static bool operator ==(Boat left, Boat right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Boat left, Boat right)
        {
            return !Equals(left, right);
        }

        // Interfaces
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Boat)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
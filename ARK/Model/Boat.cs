using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.Model
{
    public class Boat : IEquatable<Boat>
    {
        #region Enums

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

        #endregion

        // states if the boat have seating for cox
        #region Public Properties

        public bool Active { get; set; }

        public bool BoatOut
        {
            get
            {
                return this.GetActiveTrip != default(Trip);
            }
        }

        // states if the boat is not retired/dead
        public virtual ICollection<DamageForm> DamageForms { get; set; }

        public bool Damaged
        {
            // Damaged or not
            get
            {
                return this.DamageForms != null && this.DamageForms.Count != 0;
            }
        }

        public Trip GetActiveTrip
        {
            get
            {
                return this.Trips.FirstOrDefault(trip => trip.TripEndedTime == null);
            }
        }

        public bool HaveCox { get; set; }

        public int Id { get; set; }

        public string InformationString { get; set; }

        public double KilometersSailed
        {
            get
            {
                return this.Trips.Sum(t => t.Distance);
            }
        }

        public int LongDistanceTripsSailed
        {
            get
            {
                return this.Trips.Count(t => t.LongTrip);
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
                return this.Trips.Count;
            }
        }

        public bool Usable
        {
            // states if the boat is in a usable condition
            get
            {
                return this.DamageForms != null && !this.DamageForms.Any(x => !x.Functional && !x.Closed);
            }
        }

        public int Year { get; set; }

        #endregion

        #region Public Methods and Operators

        public static BoatType[] GetBoatTypes()
        {
            return new[]
                       {
                          BoatType.Inrigger, BoatType.Outrigger, BoatType.Kajak, BoatType.Gig, BoatType.Ergometer 
                       };
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

            return this.Id == other.Id;
        }

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

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Boat)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
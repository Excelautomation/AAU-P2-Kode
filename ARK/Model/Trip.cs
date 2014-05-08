using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace ARK.Model
{
    public class Trip : IEquatable<Trip>
    {
        public int Id { get; set; }
        public double Distance { get; set; }            // Distance rowed.
        public bool LongTrip { get; set; }              // bool that states if the tour is a long trip, and therefore holds no Direction.
        public DateTime TripStartTime { get; set; }     // no further explanation needed
        public DateTime? TripEndedTime { get; set; }    // no further explanation needed
        public string Direction { get; set; }           // Starting direction that is ment to be displayed douring the tour for security reasons.   
        public string Title { get; set; }               // Helping title for the user to decide the distance that the boat have sailed.
        public int CrewCount { get; set; }
 
        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }

        //Not mapped properties
        public bool TripEnded
        {
            get
            {
                if (TripEndedTime != null && TripEndedTime != TripStartTime)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public TimeSpan TimeBoatOut
        {
            get
            {
                if (this.TripEndedTime == null)
                {
                    return DateTime.Now - this.TripStartTime;
                }
                else
                {
                    return this.TripEndedTime.Value - this.TripStartTime;
                }
            }
        }

        public bool Equals(Trip other)
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
            return Equals((Trip)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Trip left, Trip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Trip left, Trip right)
        {
            return !Equals(left, right);
        }
    }
}
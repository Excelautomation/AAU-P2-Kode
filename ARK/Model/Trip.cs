using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace ARK.Model
{
    public class Trip : IEquatable<Trip>
    {
        public int Id { get; set; }
        public double Distance { get; set; }
        public bool LongTrip { get; set; }
        public DateTime TripStartTime { get; set; }
        public DateTime? TripEndedTime { get; set; }
        public string Direction { get; set; }
        public string Title { get; set; }
        
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
                //if (TripEndedTime != TripStartTime)
                //    return TripEndedTime.Value.Subtract(TripStartTime);
                //else
                    return DateTime.Now.Subtract(TripStartTime);
            }
        }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }

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
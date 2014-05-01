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
        public string Description { get; set; }

        public TimeSpan TimeBoatOut
        {
            get
            {
                var temp = DateTime.Now.Subtract(TripStartTime);
                return new TimeSpan(temp.Hours, temp.Minutes, temp.Seconds);
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
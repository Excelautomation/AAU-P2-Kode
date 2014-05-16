using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace ARK.Model
{
    public class Trip : IEquatable<Trip>
    {
        #region Public Properties

        public virtual Boat Boat { get; set; }

        public int BoatId { get; set; }

        public int CrewCount { get; set; }

        public string Direction { get; set; }

        public double Distance { get; set; }

        public int Id { get; set; }

        // Distance rowed.
        public bool LongTrip { get; set; }

        // bool that states if the tour is a long trip, and therefore holds no Direction.
        public virtual ICollection<Member> Members { get; set; }

        // Not mapped properties
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

        public string Title { get; set; }

        public bool TripEnded
        {
            get
            {
                if (this.TripEndedTime != null && this.TripEndedTime != this.TripStartTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DateTime? TripEndedTime { get; set; }

        public DateTime TripStartTime { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Trip left, Trip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Trip left, Trip right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Trip other)
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

            return this.Equals((Trip)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
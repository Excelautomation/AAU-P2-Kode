using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class LongTripForm : IEquatable<LongTripForm>
    {
        public enum BoatStatus
        {
            Awaiting,
            Accepted,
            Denied
        }

        public int Id { get; set; }
        public DateTime FormCreated { get; set; }   // Date of the freation of the form
        public DateTime StartDate { get; set; }     // Start date of the trip
        public DateTime EndDate{ get; set; }        // End date of the trip
        public string Text { get; set; }
        public BoatStatus Status { get; set; }      // statud of the Form

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }

        public bool Equals(LongTripForm other)
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
            return Equals((LongTripForm) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(LongTripForm left, LongTripForm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongTripForm left, LongTripForm right)
        {
            return !Equals(left, right);
        }
    }
}
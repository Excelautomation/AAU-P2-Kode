using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class LongDistanceForm : IEquatable<LongDistanceForm>
    {
        public enum BoatStatus
        {
            Afventer,
            Accepteret,
            Afvist
        }

        public int Id { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Text { get; set; }
        public BoatStatus Status { get; set; }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }

        public bool Equals(LongDistanceForm other)
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
            return Equals((LongDistanceForm) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(LongDistanceForm left, LongDistanceForm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongDistanceForm left, LongDistanceForm right)
        {
            return !Equals(left, right);
        }
    }
}
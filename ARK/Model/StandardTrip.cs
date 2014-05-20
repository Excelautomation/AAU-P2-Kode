using System;

namespace ARK.Model
{
    public class StandardTrip : IEquatable<StandardTrip>
    {
        public string Direction { get; set; }

        public double Distance { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public bool Equals(StandardTrip other)
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

        public static bool operator ==(StandardTrip left, StandardTrip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StandardTrip left, StandardTrip right)
        {
            return !Equals(left, right);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((StandardTrip)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
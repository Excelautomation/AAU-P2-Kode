using System;

namespace ARK.Model
{
    public class DamageType : IEquatable<DamageType>
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public bool Equals(DamageType other)
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

        public static bool operator ==(DamageType left, DamageType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DamageType left, DamageType right)
        {
            return !Equals(left, right);
        }

        // states the type of the damage
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

            return Equals((DamageType)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
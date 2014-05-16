using System;

namespace ARK.Model
{
    public class DamageType : IEquatable<DamageType>
    {
        #region Public Properties

        public int Id { get; set; }

        public string Type { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(DamageType left, DamageType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DamageType left, DamageType right)
        {
            return !Equals(left, right);
        }

        // states the type of the damage
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

            return this.Equals((DamageType)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
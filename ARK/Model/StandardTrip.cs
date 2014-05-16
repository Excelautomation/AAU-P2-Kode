using System;

namespace ARK.Model
{
    public class StandardTrip : IEquatable<StandardTrip>
    {
        #region Public Properties

        public string Direction { get; set; }

        public double Distance { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(StandardTrip left, StandardTrip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StandardTrip left, StandardTrip right)
        {
            return !Equals(left, right);
        }

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

            return this.Equals((StandardTrip)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
using System;

namespace ARK.Model
{
    public class Admin : IEquatable<Admin>
    {
        #region Public Properties

        public bool ContactDark { get; set; }

        public bool ContactTrip { get; set; }

        // Navigation property
        public virtual Member Member { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Admin left, Admin right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Admin left, Admin right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Admin other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Username, other.Username);
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

            return this.Equals((Admin)obj);
        }

        public override int GetHashCode()
        {
            return this.Username != null ? this.Username.GetHashCode() : 0;
        }

        #endregion
    }
}
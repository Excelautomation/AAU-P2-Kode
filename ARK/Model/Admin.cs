using System;

namespace ARK.Model
{
    public class Admin : IEquatable<Admin>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ContactTrip { get; set; }
        public bool ContactDark { get; set; }

        //Navigation property
        public virtual Member Member { get; set; }

        public bool Equals(Admin other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Username, other.Username);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Admin) obj);
        }

        public override int GetHashCode()
        {
            return (Username != null ? Username.GetHashCode() : 0);
        }

        public static bool operator ==(Admin left, Admin right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Admin left, Admin right)
        {
            return !Equals(left, right);
        }
    }
}
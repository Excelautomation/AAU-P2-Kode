using System;

namespace ARK.Model
{
    public class FTPInfo : IEquatable<FTPInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

        public bool Equals(FTPInfo other)
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
            return Equals((FTPInfo) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(FTPInfo left, FTPInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FTPInfo left, FTPInfo right)
        {
            return !Equals(left, right);
        }
    }
}
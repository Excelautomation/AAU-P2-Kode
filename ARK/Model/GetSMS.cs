using System;

namespace ARK.Model
{
    public class GetSMS : IEquatable<GetSMS>
    {
        public string From { get; set; }

        public bool Handled { get; set; }

        public int Id { get; set; }

        public DateTime RecievedDate { get; set; }

        public string Text { get; set; }

        public bool Equals(GetSMS other)
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

        public static bool operator ==(GetSMS left, GetSMS right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetSMS left, GetSMS right)
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

            return Equals((GetSMS)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
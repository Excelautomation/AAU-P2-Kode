using System;

namespace ARK.Model
{
    public class GetSMS : IEquatable<GetSMS>
    {
        #region Public Properties

        public string From { get; set; }

        public bool Handled { get; set; }

        public int Id { get; set; }

        public DateTime RecievedDate { get; set; }

        public string Text { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(GetSMS left, GetSMS right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetSMS left, GetSMS right)
        {
            return !Equals(left, right);
        }

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

            return this.Equals((GetSMS)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
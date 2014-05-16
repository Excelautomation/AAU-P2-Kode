using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.Model
{
    public class Member : IEquatable<Member>, IComparable<Member>
    {
        #region Static Fields

        private static readonly IEqualityComparer<Member> IdComparerInstance = new IdEqualityComparer();

        #endregion

        #region Public Properties

        public static IEqualityComparer<Member> IdComparer
        {
            get
            {
                return IdComparerInstance;
            }
        }

        public bool Active { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public DateTime Birthday { get; set; }

        public string Cellphone { get; set; }

        public string City { get; set; }

        public virtual ICollection<DamageForm> DamageForms { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string FirstName { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<LongTripForm> LongDistanceForms { get; set; }

        public bool LongTripCox { get; set; }

        public bool MayUseKayak { get; set; }

        public bool MayUseOutrigger { get; set; }

        public bool MayUseSculler { get; set; }

        public int MemberNumber { get; set; }

        public string Phone { get; set; }

        public string PhoneWork { get; set; }

        public bool Released { get; set; }

        public bool ShortTripCox { get; set; }

        public bool SwimmingTest { get; set; }

        // Navigation properties
        public virtual ICollection<Trip> Trips { get; set; }

        public int ZipCode { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Member left, Member right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Member left, Member right)
        {
            return !Equals(left, right);
        }

        public int CompareTo(Member other)
        {
            return this.Id.CompareTo(other.Id);
        }

        // Equals
        public bool Equals(Member other)
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

            return this.Equals((Member)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public double GetTotalDistance()
        {
            return this.Trips.Aggregate(0d, (a, b) => a + b.Distance);
        }

        #endregion

        private sealed class IdEqualityComparer : IEqualityComparer<Member>
        {
            #region Public Methods and Operators

            public bool Equals(Member x, Member y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.Id == y.Id;
            }

            public int GetHashCode(Member obj)
            {
                return obj.Id;
            }

            #endregion
        }
    }
}
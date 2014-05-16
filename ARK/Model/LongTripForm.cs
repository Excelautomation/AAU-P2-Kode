using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class LongTripForm : IEquatable<LongTripForm>
    {
        #region Enums

        public enum BoatStatus
        {
            Awaiting, 

            Accepted, 

            Denied
        }

        #endregion

        // Navigation properties
        #region Public Properties

        public virtual Boat Boat { get; set; }

        public int? BoatId { get; set; }

        public string CampSites { get; set; }

        public string DistancesPerDay { get; set; }

        public DateTime FormCreated { get; set; }

        public int Id { get; set; }

        public virtual List<Member> Members { get; set; }

        public DateTime PlannedEndDate { get; set; }

        public DateTime PlannedStartDate { get; set; }

        public virtual Member ResponsibleMember { get; set; }

        public int ResponsibleMemberId { get; set; }

        public BoatStatus Status { get; set; }

        public string TourDescription { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(LongTripForm left, LongTripForm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongTripForm left, LongTripForm right)
        {
            return !Equals(left, right);
        }

        public bool Equals(LongTripForm other)
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

            return this.Equals((LongTripForm)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        #endregion
    }
}
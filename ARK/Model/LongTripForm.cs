using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class LongTripForm : IEquatable<LongTripForm>
    {
        public enum BoatStatus
        {
            Awaiting,
            Accepted,
            Denied
        }

        public int Id { get; set; }
        public DateTime FormCreated { get; set; }   // Date of the creation of the form
        public DateTime PlannedStartDate { get; set; }     // Start date of the trip
        public DateTime PlannedEndDate{ get; set; }        // End date of the trip
        public string TourDescription { get; set; }
        public string DistancesPerDay { get; set; } // eventually an integer!!!!!!! 
        public string CampSites { get; set; }       // 
        public BoatStatus Status { get; set; }      // status of the Form

        //Foreign key
        public int? BoatId { get; set; }
        public int ResponsibleMemberId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual List<Member> Members { get; set; }
        public virtual Member ResponsibleMember { get; set; }

        public bool Equals(LongTripForm other)
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
            return Equals((LongTripForm) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(LongTripForm left, LongTripForm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongTripForm left, LongTripForm right)
        {
            return !Equals(left, right);
        }
    }
}
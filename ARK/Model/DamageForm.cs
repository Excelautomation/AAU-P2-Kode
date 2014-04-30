using System;

namespace ARK.Model
{
    public class DamageForm : IEquatable<DamageForm>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string NeededMaterials { get; set; }
        public bool Functional { get; set; }
        public bool Closed { get; set; }

        //Foreign Keys
        public int RegisteringMemberId { get; set; }
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual Member RegisteringMember { get; set; }

        public bool Equals(DamageForm other)
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
            return Equals((DamageForm) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(DamageForm left, DamageForm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DamageForm left, DamageForm right)
        {
            return !Equals(left, right);
        }
    }
}
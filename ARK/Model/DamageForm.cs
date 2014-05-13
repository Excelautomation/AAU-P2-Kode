using System;

namespace ARK.Model
{
    public class DamageForm : IEquatable<DamageForm>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }          // Date for the damage
        public string Type { get; set; }            // Damagetype
        public string Description { get; set; }     // Further description of the damage. what en where etc.
        public bool Functional { get; set; }        // bool that states if the boat is functional
        public bool Closed { get; set; }            // bool that states if the damageform is invalid or if the damage have ben repaired
        
        public DamageForm()
        {
            Date = DateTime.Now;
        }

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
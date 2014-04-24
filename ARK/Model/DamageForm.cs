using System;

namespace ARK.Model
{
    public class DamageForm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string NeededMaterials { get; set; }
        public bool Closed { get; set; }

        //Foreign Keys
        public int RegisteringMemberId { get; set; }
        public int BoatId { get; set; }
        
        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual Member RegisteringMember { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class LongDistanceForm
    {
        public int Id { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Text { get; set; }
        public bool? Approved { get; set; }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
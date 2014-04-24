using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public bool LongTrip { get; set; }
        public DateTime TripStartTime { get; set; }
        public DateTime TripEndedTime { get; set; }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public double Distance { get; set; }
        public bool LongTrip { get; set; }
        public DateTime TripStartTime { get; set; }
        public DateTime TripEndedTime { get; set; }
        public string Direction { get; set; }
        public string Description { get; set; }

        public TimeSpan TimeBoatOut
        {
            get
            {
                if (TripEndedTime != null)
                {
                    return DateTime.Now.Subtract(TripStartTime);
                }
                return TimeBoatOut;
            }
        }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ARK.Diagrams
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trips
    {
        public Trips()
        {
            this.Members = new HashSet<Members>();
        }
    
        public int Id { get; set; }
        public double Distance { get; set; }
        public bool LongTrip { get; set; }
        public System.DateTime TripStartTime { get; set; }
        public Nullable<System.DateTime> TripEndedTime { get; set; }
        public int BoatId { get; set; }
        public string Direction { get; set; }
        public string Title { get; set; }
        public int CrewCount { get; set; }
    
        public virtual Boats Boats { get; set; }
        public virtual TripWarningSms TripWarningSms { get; set; }
        public virtual ICollection<Members> Members { get; set; }
    }
}

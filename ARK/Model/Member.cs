using System;
using System.Collections.Generic;

namespace ARK.Model
{
    public class Member
    {
        public int Id { get; set; }
        public int MemberNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string PhoneWork { get; set; }
        public string Cellphone { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public DateTime Birthday { get; set; }
        public bool Released { get; set; }
        public bool SwimmingTest { get; set; }
        public bool ShortTripCox { get; set; }
        public bool LongTripCox { get; set; }
        public bool MayUseSculler { get; set; }
        public bool MayUseOutrigger { get; set; }
        public bool MayUseKayak { get; set; }

        //Navigation properties
        public virtual ICollection<DamageForm> DamageForms { get; set; }
        public virtual ICollection<LongDistanceForm> LongDistanceForms { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}

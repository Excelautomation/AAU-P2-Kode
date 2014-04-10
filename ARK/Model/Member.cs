using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.Model
{
    public class Member
    {
        public Member(XMLMembers.datarootAktiveMedlemmer medlemXML)
        {
        }

        public int MemberNumber { get; set; }
        public int ID { get; set; }
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
    }
}

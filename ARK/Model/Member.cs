using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;
using System.Text.RegularExpressions;

namespace ARK.Model
{
    public class Member
    {
        public Member()
        {
        }

        public Member(XMLMembers.datarootAktiveMedlemmer memberXML)
        {
            this.MemberNumber = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.MedlemsNr));
            this.Id = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.ID));
            this.FirstName = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Fornavn);
            this.LastName = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Efternavn);
            this.Address1 = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse1);
            this.Address2 = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse2);
            this.ZipCode = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.PostNr));
            this.City = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.By);

            string phone = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Telefon);
            if (phone != null)
            {
                this.Phone = Regex.Replace(phone, @"[^0-9]", "");
            }

            string phoneWork = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonArbejde);
            if (phoneWork != null)
            {
                this.PhoneWork = Regex.Replace(phoneWork, @"[^0-9]", "");
            }

            string cellPhone = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonMobil);
            if (cellPhone != null)
            {
                this.Cellphone = Regex.Replace(cellPhone, @"[^0-9]", "");
            }

            this.Email1 = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.EMail);
            this.Email2 = (string)memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.EMail2);
            this.Birthday = Convert.ToDateTime(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Fødselsdato));
            this.Released = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Frigivet)) == 1;
            this.SwimmingTest = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Svømmeprøve)) == 1;
            this.ShortTripCox = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Korttursstyrmand)) == 1;
            this.LongTripCox = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Langtursstyrmand)) == 1;
            this.MayUseSculler = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Scullerret)) == 1;
            this.MayUseOutrigger = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Outriggerret)) == 1;
            this.MayUseKayak = Convert.ToInt32(memberXML.GetObjFromName(XMLMembers.ItemsChoiceType.Kajakret)) == 1;
            this.Trips = new List<Trip>();
            this.LongDistanceForms = new List<LongDistanceForm>();
            this.DamageForms = new List<DamageForm>();
        }

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

        public virtual ICollection<DamageForm> DamageForms { get; set; }
        public virtual ICollection<LongDistanceForm> LongDistanceForms { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}

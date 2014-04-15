using ARK.Model;
using ARK.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ARK.ViewModel
{
    public class BeginTripViewModel : Base.ViewModel
    {
        private bool _enableMembers;
        private List<Boat> _boats = new List<Boat>();
        private List<Member> _members = new List<Member>();
        private Boat _boat;
        private Member _member;

        public bool EnableMembers
        {
            get { return _enableMembers; }
            set { _enableMembers = value; Notify("EnableMembers"); }
        }

        public ICommand SelectedChange
        {
            get
            {
                return GetCommand<Boat>(e =>
                {
                    EnableMembers = true;
                    Boat = e;
                });
            }
        }
        
        public BeginTripViewModel()
        {
            // Load data
            using (var db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat);
                Members = new List<Member>(db.Member);
            }
        }

        public List<Boat> Boats
        {
            get { return _boats; }
            set { _boats = value; Notify("Boats"); }
        }

        public List<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify("Members"); }
        }

        public string BoatHeadline { get { return "Båd"; } }

        public List<Boat> BoatContent
        {
            get
            {
                if (Boat == null) return new List<Boat>();

                return new List<Boat> { Boat };  
            }
        }

        public string MemberHeadline { get { return "Deltagere"; } }

        public List<Member> MemberContent
        {
            get
            {
                if (Member == null) return new List<Member>();

                return new List<Member> { Member }; 
            }
        }

        public Boat Boat 
        {
            get { return _boat;  }
            set { _boat = value; Notify("Boat"); Notify("BoatContent"); }
        }

        public Member Member
        {
            get { return _member; }
            set { _member = value; Notify("Member"); Notify("MemberContent"); }
        }
    }
}

using System.Collections.Generic;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using System.Collections.ObjectModel;

namespace ARK.ViewModel
{
    public class BeginTripViewModel : Base.ViewModel
    {
        private Boat _boat;
        private List<Boat> _boats = new List<Boat>();
        private List<Member> _members = new List<Member>();
        private bool _enableMembers;
        private Member _member;

        public BeginTripViewModel()
        {
            // Load data
            using (DbArkContext db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat);
                Members = new List<Member>(db.Member);
            }
        }

        public ObservableCollection<Member> MemberCollection
        {
            get;
            set;
        }

        public bool EnableMembers
        {
            get 
            { 
                return _enableMembers; 
        }
            set 
            { 
                _enableMembers = value; 
                Notify(); 
            }
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

        public List<Boat> Boats
        {
            get 
            { 
                return _boats; 
        }
            set 
            { 
                _boats = value; 
                Notify(); 
            }
        }

        public List<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify(); }
        }

        public ICommand MemberSelected
        {
            get
            {
                return GetCommand<IList<Member>>(e =>
                    {
                        MemberCollection = new ObservableCollection<Member>(e);
                    });
            }
        }

        public string BoatHeadline { get { return "Båd"; } }

        public List<Boat> BoatContent
        {
            get
            {
                if (Boat == null)
                {
                    return new List<Boat>();
                }

                return new List<Boat> { Boat };  
            }
        }

        public string MemberHeadline
        {
            get { return "Deltagere"; }
        }

        public List<Member> MemberContent
        {
            get
            {
                if (Member == null)
                {
                    return new List<Member>();
                }

                return new List<Member> { Member }; 
            }
        }

        public Boat Boat 
        {
            get { return _boat; }
            set { _boat = value; Notify(); NotifyProp("BoatContent"); }
        }

        public Member Member
        {
            get { return _member; }
            set { _member = value; Notify(); NotifyProp("MemberContent"); }
        }
    }
}

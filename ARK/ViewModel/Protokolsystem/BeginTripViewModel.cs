using System.Collections.Generic;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base.Keyboard;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : Base.ViewModel, IKeyboardChange
    {
        private Boat _boat;
        private List<Boat> _boats = new List<Boat>();
        private List<Member> _members = new List<Member>();
        private bool _enableMembers;
        private List<Member> _member;
        private string _keyboardToggleText;

        public BeginTripViewModel()
        {
            // Load data
            using (DbArkContext db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat).Where(x => x.Active).OrderBy(x => x.NumberofSeats).ToList();
                Members = new List<Member>(db.Member).Select(x =>
                    {
                        x.FirstName = x.FirstName.Trim();
                        return x;
                    }).OrderBy(x => x.FirstName).ToList();
            }
        }

        public bool EnableMembers
        {
            get { return _enableMembers; }
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
            get { return _boats; }
            set 
            { 
                _boats = value; 
                Notify(); 
            }
        }

        public List<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                Notify();
        }
        }

        public ICommand MemberSelected
        {
            get
            {
                return GetCommand<IList>(e =>
                    {
                        Member = e.Cast<Member>().ToList();
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

                return new List<Boat> {Boat};
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

                return new List<Member>(Member);
            }
        }

        public Boat Boat 
        {
            get { return _boat; }
            set
            {
                _boat = value;
                Notify();
                NotifyProp("BoatContent");
            }
        }

        public List<Member> Member
        {
            get { return _member; }
            set
            {
                _member = value;
                Notify();
                NotifyProp("MemberContent");
            }
        }

        public string KeyboardToggleText
        {
            get
            {
                if (string.IsNullOrEmpty(_keyboardToggleText)) return "VIS\nTASTATUR";

                return _keyboardToggleText;
            }
            set { _keyboardToggleText = value; Notify(); }
        }
    }
}

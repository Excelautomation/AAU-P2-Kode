using System.Collections.Generic;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base.Keyboard;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : Base.ViewModel, IKeyboardHandler
    {
        private Boat _selectedBoat;
        private List<Boat> _boats = new List<Boat>();
        private List<Member> _members = new List<Member>();
        private bool _enableMembers;
        private readonly ObservableCollection<Member> _selectedMembers;
        private IList _test;
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
                _selectedMembers = new ObservableCollection<Member>();
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

        public ICommand BoatSelected
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

        public ICommand MemberSelected
        {
            get
            {
                return GetCommand<IList>(e =>
                    {
                        _test = e;
                    });
            }
        }

        public ObservableCollection<Boat> SelectedBoat
        {
            get
            {
                if (Boat == null)
                {
                    return new ObservableCollection<Boat>();
                }

                return new ObservableCollection<Boat> { Boat };
            }
        }

        public Boat Boat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
                Notify();
                NotifyProp("BoatContent");
            }
        }

        public ObservableCollection<Member> SelectedMembers
        {
            get 
            {
                if (_test == null)
                {
                    return new ObservableCollection<Member>();
                }
                return new ObservableCollection<Member>(_test.Cast<Member>()); 
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

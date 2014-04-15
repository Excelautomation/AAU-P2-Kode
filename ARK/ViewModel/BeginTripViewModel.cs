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

        public bool EnableMembers
        {
            get { return _enableMembers; }
            set { _enableMembers = value; Notify("EnableMembers"); }
        }

        public ICommand SelectedChange
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    EnableMembers = true;
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
    }
}

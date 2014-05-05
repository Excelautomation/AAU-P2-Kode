using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using System.Windows.Input;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem
{
    class CreateInjuryViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private Member _selectedMember;
        private List<Member> _members;
        private Boat _selectedBoat;
        private List<Boat> _boats;

        // constructor
        public CreateInjuryViewModel()
        {
            var db = DbArkContext.GetDbContext();
            _members = db.Member.Where(m => true).ToList();
            _boats = db.Boat.Where(b => true).ToList();
        }

        // Properties
        // injury reporter
        public Member SelectedMember
        {
            get { return _selectedMember; }
            set 
            {
                _selectedMember = value; 
                Notify(); 
            }
        }

        public List<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify(); }
        }

        // injured boat
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set { _selectedBoat = value; Notify(); }
        }

        public List<Boat> Boats
        {
            get { return _boats; }
            set { _boats = value; Notify(); }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    SelectedMember = e;
                });
            }
        }

        public ICommand BoatSelectionChanged
        {
            get
            {
                return GetCommand<Boat>(b =>
                {
                    SelectedBoat = b;
                });
            }
        }

        public string Comment { get; set; }

        public bool IsFunctional { get; set; }
    }
}

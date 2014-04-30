using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : KeyboardContentViewModelBase
    {
        // Fields
        private List<Member> _members = new List<Member>();
        private Member _selectedMember;
        private List<Trip> _trips;

        // Constructor
        public DistanceStatisticsViewModel()
        {
            var db = DbArkContext.GetDbContext();
            
            // Load data
            _members = new List<Member>(db.Member)
            .Select(x => { x.FirstName = x.FirstName.Trim(); return x; })
            .OrderBy(x => x.FirstName)
            .ToList();
        }

        // Properties
        public List<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                Notify();
            }
        }

        public Member SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember = value; Notify(); getLatestTrips(); }
        }

        private void getLatestTrips()
        {
            DbArkContext db = DbArkContext.GetDbContext();
            
            Trips = db.Trip.ToList().Where(t => t.Members.Contains(SelectedMember)).ToList();
        }

        public List<Trip> Trips
        {
            get { return _trips; }
            set { _trips = value; Notify(); }
        }

        //
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
    }
}
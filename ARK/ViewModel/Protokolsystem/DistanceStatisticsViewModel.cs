using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Forms.VisualStyles;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private readonly ObservableCollection<Tuple<Member, double>> _memberKmCollection;
        private Member _selectedMember;
        private List<Trip> _trips = new List<Trip>();

        // Constructor
        public DistanceStatisticsViewModel()
        {
            var db = DbArkContext.GetDbContext();

            DateTime lowerTimeLimit = new DateTime();
            DateTime upperTimeLimit = DateTime.Now;
            // Load data
            var temp = db.Member
                .OrderBy(x => x.FirstName)
                .Include(m => m.Trips)
                .AsEnumerable();

            _memberKmCollection =
                new ObservableCollection<Tuple<Member, double>>
                    (temp.Select((val, i) => new Tuple<Member, double>(val, val.Trips
                        .Where(t => t.TripStartTime > lowerTimeLimit && t.TripStartTime < upperTimeLimit)
                        .Sum(t => t.Distance))));
        }

        public Member SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember = value; Notify(); GetLatestTrips(); }
        }

        public ObservableCollection<Tuple<Member, double>> MemberKmCollection
        {
            get { return _memberKmCollection; }
        }

        private void GetLatestTrips()
        {
            Trips = SelectedMember.Trips.ToList();
        }

        public List<Trip> Trips
        {
            get { return _trips; }
            set { _trips = value; Notify(); }
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
    }
}
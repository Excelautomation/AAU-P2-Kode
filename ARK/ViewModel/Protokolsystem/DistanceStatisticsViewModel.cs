using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Forms.VisualStyles;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
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
            var members = db.Member
                .OrderBy(x => x.FirstName)
                .Include(m => m.Trips)
                .AsEnumerable();

            _memberKmCollection =
                new ObservableCollection<Tuple<Member, double>>
                    (members.Select((val, i) => new Tuple<Member, double>(val, val.Trips
                        .Where(t => t.TripStartTime > lowerTimeLimit && t.TripStartTime < upperTimeLimit)
                        .Sum(t => t.Distance)))
                        .OrderByDescending(x => x.Item2));
            SelectedMember = _memberKmCollection.Select(x => x.Item1).First(x => true);

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, false);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
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

        #region Filter
        public System.Windows.FrameworkElement Filter
        {
            get { return new DistanceStatisticsFilters(); }
        }

        private void ResetFilter()
        {
            
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
                (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
                return;

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                
            }

            // Search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                
            }
        }
        #endregion
    }
}
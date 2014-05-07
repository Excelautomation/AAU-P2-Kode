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
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        // Fields
        private readonly ObservableCollection<MemberDistanceViewModel> _memberKmCollectionFiltered;
        private MemberDistanceViewModel _selectedMember;
        private List<Trip> _tripsFiltered = new List<Trip>();

        // Constructor
        public DistanceStatisticsViewModel()
        {
            var db = DbArkContext.GetDbContext();

            var lowerTimeLimit = new DateTime();
            var upperTimeLimit = DateTime.Now;

            // Load data
            var members = db.Member
                .OrderBy(x => x.FirstName)
                .Include(m => m.Trips)
                .AsEnumerable();

            _memberKmCollectionFiltered =
                new ObservableCollection<MemberDistanceViewModel>
                    (members.Select((member, i) => new MemberDistanceViewModel(member, member.Trips
                        .Where(t => t.TripStartTime > lowerTimeLimit && t.TripStartTime < upperTimeLimit)
                        .Sum(t => t.Distance)))
                        .OrderByDescending(trips => trips.Distance));
            SelectedMember = _memberKmCollectionFiltered.First();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, false);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public MemberDistanceViewModel SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember = value; Notify(); }
        }

        public ObservableCollection<MemberDistanceViewModel> MemberKmCollectionFiltered
        {
            get { return _memberKmCollectionFiltered; }
        }

        public List<Trip> TripsFiltered
        {
            get { return _tripsFiltered; }
            set { _tripsFiltered = value; Notify(); }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return GetCommand<MemberDistanceViewModel>(e =>
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

        private void UpdateFilter()
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
using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class MemberDistanceViewModel : ViewModelBase
    {
        private double _distance;
        private IEnumerable<TripViewModel> _filteredTrips;
        private Member _member;
        private readonly IEnumerable<TripViewModel> _trips;

        public MemberDistanceViewModel(Member member)
        {
            Member = member;
            _trips =
                member.Trips.Where(t => t.TripEndedTime != null)
                    .OrderByDescending(t => t.TripEndedTime)
                    .Select(t => new TripViewModel(t));

            ResetFilter();
        }

        public Member Member
        {
            get { return _member; }
            private set
            {
                _member = value;

                Notify();
            }
        }

        public IEnumerable<TripViewModel> FilteredTrips
        {
            get { return _filteredTrips; }
            private set
            {
                _filteredTrips = value;
                Notify();

                UpdateDistance();
            }
        }

        public double Distance
        {
            get { return _distance; }
            private set
            {
                _distance = value;
                Notify();
            }
        }

        public void ResetFilter()
        {
            FilteredTrips = _trips.ToList();
        }

        public void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filter
            ResetFilter();
            
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
                (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                UpdateDistance();
                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                FilteredTrips = FilterContent.FilterItems(FilteredTrips, args.FilterEventArgs);
            }
        }

        public void UpdateDistance()
        {
            var lowerTimeLimit = new DateTime();
            var upperTimeLimit = DateTime.Now;

            Distance = FilteredTrips
                .Where(t => t.Trip.TripEndedTime != null && t.Trip.TripStartTime > lowerTimeLimit && t.Trip.TripStartTime < upperTimeLimit)
                .Sum(t => t.Trip.Distance);
        }
    }
}
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
        private IEnumerable<Trip> _filteredTrips;
        private Member _member;
        private readonly IEnumerable<Trip> _trips;

        public MemberDistanceViewModel(Member member)
        {
            Member = member;
            _trips = member.Trips;

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

        public IEnumerable<Trip> FilteredTrips
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

        private void ResetFilter()
        {
            FilteredTrips = _trips.ToList();
        }

        public void UpdateFilter(FilterChangedEventArgs args)
        {
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

        private void UpdateDistance()
        {
            var lowerTimeLimit = new DateTime();
            var upperTimeLimit = DateTime.Now;

            Distance = FilteredTrips
                .Where(t => t.TripStartTime > lowerTimeLimit && t.TripStartTime < upperTimeLimit)
                .Sum(t => t.Distance);
        }
    }
}
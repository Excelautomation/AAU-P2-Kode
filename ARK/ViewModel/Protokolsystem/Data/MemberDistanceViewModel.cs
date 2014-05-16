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
        #region Fields

        private readonly IEnumerable<TripViewModel> _trips;

        private double _distance;

        private IEnumerable<TripViewModel> _filteredTrips;

        private Member _member;

        private int _position;

        #endregion

        #region Constructors and Destructors

        public MemberDistanceViewModel(Member member)
        {
            this.Member = member;
            this._trips =
                member.Trips.Where(t => t.TripEndedTime != null)
                    .OrderByDescending(t => t.TripEndedTime)
                    .Select(t => new TripViewModel(t));

            this.ResetFilter();
        }

        #endregion

        #region Public Properties

        public double Distance
        {
            get
            {
                return this._distance;
            }

            private set
            {
                this._distance = value;
                this.Notify();
            }
        }

        public IEnumerable<TripViewModel> FilteredTrips
        {
            get
            {
                return this._filteredTrips;
            }

            private set
            {
                this._filteredTrips = value;
                this.Notify();

                this.UpdateDistance();
            }
        }

        public Member Member
        {
            get
            {
                return this._member;
            }

            private set
            {
                this._member = value;

                this.Notify();
            }
        }

        public int Position
        {
            get
            {
                return this._position;
            }

            set
            {
                this._position = value;
                this.Notify();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ResetFilter()
        {
            this.FilteredTrips = this._trips.ToList();
        }

        public void UpdateDistance()
        {
            var lowerTimeLimit = new DateTime();
            var upperTimeLimit = DateTime.Now;

            this.Distance =
                this.FilteredTrips.Where(
                    t =>
                    t.Trip.TripEndedTime != null && t.Trip.TripStartTime > lowerTimeLimit
                    && t.Trip.TripStartTime < upperTimeLimit).Sum(t => t.Trip.Distance);
        }

        public void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filter
            this.ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                this.UpdateDistance();
                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.FilteredTrips = FilterContent.FilterItems(this.FilteredTrips, args.FilterEventArgs);
            }
        }

        #endregion
    }
}
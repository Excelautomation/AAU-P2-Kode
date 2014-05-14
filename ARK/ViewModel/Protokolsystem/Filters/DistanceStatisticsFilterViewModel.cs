using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class DistanceStatisticsFilterViewModel : FilterViewModelBase
    {
        private bool _dateTimeAll;
        private bool _dateTimeDay;
        private DateTime? _dateTimeFrom;
        private bool _dateTimeMonth;
        private DateTime? _dateTimeTo;
        private bool _dateTimeWeek;
        private bool _statisticsAll;
        private bool _statisticsErgometer;
        private bool _statisticsGig;
        private bool _statisticsInrigger;
        private bool _statisticsKajak;
        private bool _statisticsOutrigger;

        public DistanceStatisticsFilterViewModel()
        {
            CurrentBoatType = new CategoryFilter<TripViewModel>(trip => true);
            CurrentDateTimeFilter = new DateTimeFilter();

            StatisticsAll = true;
            DateTimeAll = true;

            UpdateFilter();
        }

        public CategoryFilter<TripViewModel> CurrentBoatType { get; set; }
        public DateTimeFilter CurrentDateTimeFilter { get; set; }

        public bool DateTimeAll
        {
            get { return _dateTimeAll; }
            set
            {
                _dateTimeAll = value;
                if (value)
                {
                    DateTimeFrom = null;
                    DateTimeTo = null;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        public bool DateTimeDay
        {
            get { return _dateTimeDay; }
            set
            {
                _dateTimeDay = value;

                if (value)
                {
                    DateTimeFrom = DateTime.Now.AddDays(-1);
                    DateTimeTo = DateTime.Now;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        public bool DateTimeWeek
        {
            get { return _dateTimeWeek; }
            set
            {
                _dateTimeWeek = value;

                if (value)
                {
                    DateTimeFrom = DateTime.Now.AddDays(-7);
                    DateTimeTo = DateTime.Now;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        public bool DateTimeMonth
        {
            get { return _dateTimeMonth; }
            set
            {
                _dateTimeMonth = value;

                if (value)
                {
                    DateTimeFrom = DateTime.Now.AddMonths(-1);
                    DateTimeTo = DateTime.Now;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        private DateTime? DateTimeFrom
        {
            get { return _dateTimeFrom; }
            set
            {
                _dateTimeFrom = value;
                Notify();
            }
        }

        private DateTime? DateTimeTo
        {
            get { return _dateTimeTo; }
            set
            {
                _dateTimeTo = value;
                Notify();
            }
        }

        public DateTime? DateTimeFromPicker
        {
            get { return DateTimeFrom.HasValue ? DateTimeFrom.Value : DateTime.MinValue; }
            set
            {
                DateTimeFrom = value;
                UpdateDateTime();
            }
        }

        public DateTime? DateTimeToPicker
        {
            get { return DateTimeTo; }
            set
            {
                DateTimeTo = value;
                UpdateDateTime();
            }
        }

        public bool StatisticsAll
        {
            get { return _statisticsAll; }
            set
            {
                _statisticsAll = value;
                if (value)
                    UpdateCategory(trip => true);

                Notify();
            }
        }

        public bool StatisticsErgometer
        {
            get { return _statisticsErgometer; }
            set
            {
                _statisticsErgometer = value;
                if (value)
                    UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Ergometer);

                Notify();
            }
        }

        public bool StatisticsKajak
        {
            get { return _statisticsKajak; }
            set
            {
                _statisticsKajak = value;
                if (value)
                    UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Kajak);

                Notify();
            }
        }

        public bool StatisticsInrigger
        {
            get { return _statisticsInrigger; }
            set
            {
                _statisticsInrigger = value;
                if (value)
                    UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Inrigger);

                Notify();
            }
        }

        public bool StatisticsGig
        {
            get { return _statisticsGig; }
            set
            {
                _statisticsGig = value;
                if (value)
                    UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Gig);

                Notify();
            }
        }

        public bool StatisticsOutrigger
        {
            get { return _statisticsOutrigger; }
            set
            {
                _statisticsOutrigger = value;
                if (value)
                    UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Outrigger);

                Notify();
            }
        }

        private void UpdateCategory(Func<TripViewModel, bool> filter)
        {
            CurrentBoatType.Filter = filter;

            UpdateFilter();
        }

        private void UpdateDateTime()
        {
            CurrentDateTimeFilter.StartDate = DateTimeFrom;
            CurrentDateTimeFilter.EndDate = DateTimeTo;

            UpdateFilter();
        }

        private void UpdateFilter()
        {
            base.OnFilterChanged();
        }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> {CurrentBoatType, CurrentDateTimeFilter};
        }

        public class DateTimeFilter : IFilter
        {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (typeof (T) != typeof (TripViewModel))
                    return items;

                IEnumerable<TripViewModel> trips = items.Cast<TripViewModel>().ToList();
                return trips.Where(o => (!StartDate.HasValue || o.Trip.TripStartTime.Date >= StartDate.Value.Date) &&
                    (!EndDate.HasValue || o.Trip.TripStartTime.Date <= EndDate.Value.Date)).Cast<T>();
            }
        }
    }
}
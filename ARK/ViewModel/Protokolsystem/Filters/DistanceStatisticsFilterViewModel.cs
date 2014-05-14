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
        private bool _dateTimeMonth;
        private bool _dateTimeWeek;
        private bool _statisticsAll;
        private bool _statisticsErgometer;
        private bool _statisticsGig;
        private bool _statisticsInrigger;
        private bool _statisticsKajak;
        private bool _statisticsOutrigger;
        private bool _dateTimeYear;
        private bool _dateTimeHalfYear;

        public DistanceStatisticsFilterViewModel()
        {
            CurrentBoatType = new CategoryFilter<TripViewModel>(trip => true);
            CurrentDateTimeFilter = new DateTimeFilter();

            DateTimeFromPicker = DateTime.Now;
            DateTimeToPicker = DateTime.Now;

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
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = false;

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
                    DateTimeFromPicker = DateTime.Now.AddDays(-1);
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = true;

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
                    DateTimeFromPicker = DateTime.Now.AddDays(-7);
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = true;

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
                    DateTimeFromPicker = DateTime.Now.AddMonths(-1);
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = true;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        public bool DateTimeHalfYear
        {
            get { return _dateTimeHalfYear; }
            set
            {
                _dateTimeHalfYear = value;

                if (value)
                {
                    DateTimeFromPicker = DateTime.Now.AddMonths(-6);
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = true;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        public bool DateTimeYear
        {
            get { return _dateTimeYear; }
            set
            {
                _dateTimeYear = value;

                if (value)
                {
                    DateTimeFromPicker = DateTime.Now.AddYears(-1);
                    DateTimeToPicker = DateTime.Now;

                    FilterByDates = true;

                    UpdateDateTime();
                }

                Notify();
            }
        }

        private DateTime? DateTimeFrom { get; set; }
        private DateTime? DateTimeTo { get; set; }

        public DateTime? DateTimeFromPicker
        {
            get { return DateTimeFrom; }
            set
            {
                DateTimeFrom = value;

                //FilterByDates = true;
                //UpdateDateTime();
                Notify();
            }
        }

        public DateTime DateTimeToPicker
        {
            get { return DateTimeTo.HasValue ? DateTimeTo.Value : DateTime.Now; }
            set
            {
                DateTimeTo = value;

                //UpdateDateTime();
                Notify();
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
            CurrentDateTimeFilter.StartDate = FilterByDates ? DateTimeFrom : DateTime.MinValue;
            CurrentDateTimeFilter.EndDate = DateTimeTo;

            UpdateFilter();
        }

        public bool FilterByDates { get; set; }

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
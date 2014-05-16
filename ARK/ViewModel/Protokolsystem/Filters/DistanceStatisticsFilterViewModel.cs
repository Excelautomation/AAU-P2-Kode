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
        #region Fields

        private bool _dateTimeAll;

        private bool _dateTimeDay;

        private bool _dateTimeHalfYear;

        private bool _dateTimeMonth;

        private bool _dateTimeWeek;

        private bool _dateTimeYear;

        private bool _statisticsAll;

        private bool _statisticsErgometer;

        private bool _statisticsGig;

        private bool _statisticsInrigger;

        private bool _statisticsKajak;

        private bool _statisticsOutrigger;

        #endregion

        #region Constructors and Destructors

        public DistanceStatisticsFilterViewModel()
        {
            this.CurrentBoatType = new CategoryFilter<TripViewModel>(trip => true);
            this.CurrentDateTimeFilter = new DateTimeFilter();

            this.DateTimeFromPicker = DateTime.Now;
            this.DateTimeToPicker = DateTime.Now;

            this.StatisticsAll = true;
            this.DateTimeAll = true;

            this.UpdateFilter();
        }

        #endregion

        #region Public Properties

        public CategoryFilter<TripViewModel> CurrentBoatType { get; set; }

        public DateTimeFilter CurrentDateTimeFilter { get; set; }

        public bool DateTimeAll
        {
            get
            {
                return this._dateTimeAll;
            }

            set
            {
                this._dateTimeAll = value;
                if (value)
                {
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = false;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public bool DateTimeDay
        {
            get
            {
                return this._dateTimeDay;
            }

            set
            {
                this._dateTimeDay = value;

                if (value)
                {
                    this.DateTimeFromPicker = DateTime.Now.AddDays(-1);
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = true;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public DateTime? DateTimeFromPicker
        {
            get
            {
                return this.DateTimeFrom;
            }

            set
            {
                this.DateTimeFrom = value;

                // FilterByDates = true;
                // UpdateDateTime();
                this.Notify();
            }
        }

        public bool DateTimeHalfYear
        {
            get
            {
                return this._dateTimeHalfYear;
            }

            set
            {
                this._dateTimeHalfYear = value;

                if (value)
                {
                    this.DateTimeFromPicker = DateTime.Now.AddMonths(-6);
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = true;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public bool DateTimeMonth
        {
            get
            {
                return this._dateTimeMonth;
            }

            set
            {
                this._dateTimeMonth = value;

                if (value)
                {
                    this.DateTimeFromPicker = DateTime.Now.AddMonths(-1);
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = true;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public DateTime DateTimeToPicker
        {
            get
            {
                return this.DateTimeTo.HasValue ? this.DateTimeTo.Value : DateTime.Now;
            }

            set
            {
                this.DateTimeTo = value;

                // UpdateDateTime();
                this.Notify();
            }
        }

        public bool DateTimeWeek
        {
            get
            {
                return this._dateTimeWeek;
            }

            set
            {
                this._dateTimeWeek = value;

                if (value)
                {
                    this.DateTimeFromPicker = DateTime.Now.AddDays(-7);
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = true;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public bool DateTimeYear
        {
            get
            {
                return this._dateTimeYear;
            }

            set
            {
                this._dateTimeYear = value;

                if (value)
                {
                    this.DateTimeFromPicker = DateTime.Now.AddYears(-1);
                    this.DateTimeToPicker = DateTime.Now;

                    this.FilterByDates = true;

                    this.UpdateDateTime();
                }

                this.Notify();
            }
        }

        public bool FilterByDates { get; set; }

        public bool StatisticsAll
        {
            get
            {
                return this._statisticsAll;
            }

            set
            {
                this._statisticsAll = value;
                if (value)
                {
                    this.UpdateCategory(trip => true);
                }

                this.Notify();
            }
        }

        public bool StatisticsErgometer
        {
            get
            {
                return this._statisticsErgometer;
            }

            set
            {
                this._statisticsErgometer = value;
                if (value)
                {
                    this.UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Ergometer);
                }

                this.Notify();
            }
        }

        public bool StatisticsGig
        {
            get
            {
                return this._statisticsGig;
            }

            set
            {
                this._statisticsGig = value;
                if (value)
                {
                    this.UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Gig);
                }

                this.Notify();
            }
        }

        public bool StatisticsInrigger
        {
            get
            {
                return this._statisticsInrigger;
            }

            set
            {
                this._statisticsInrigger = value;
                if (value)
                {
                    this.UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Inrigger);
                }

                this.Notify();
            }
        }

        public bool StatisticsKajak
        {
            get
            {
                return this._statisticsKajak;
            }

            set
            {
                this._statisticsKajak = value;
                if (value)
                {
                    this.UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Kajak);
                }

                this.Notify();
            }
        }

        public bool StatisticsOutrigger
        {
            get
            {
                return this._statisticsOutrigger;
            }

            set
            {
                this._statisticsOutrigger = value;
                if (value)
                {
                    this.UpdateCategory(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Outrigger);
                }

                this.Notify();
            }
        }

        #endregion

        #region Properties

        private DateTime? DateTimeFrom { get; set; }

        private DateTime? DateTimeTo { get; set; }

        #endregion

        #region Public Methods and Operators

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this.CurrentBoatType, this.CurrentDateTimeFilter };
        }

        #endregion

        #region Methods

        private void UpdateCategory(Func<TripViewModel, bool> filter)
        {
            this.CurrentBoatType.Filter = filter;

            this.UpdateFilter();
        }

        private void UpdateDateTime()
        {
            this.CurrentDateTimeFilter.StartDate = this.FilterByDates ? this.DateTimeFrom : DateTime.MinValue;
            this.CurrentDateTimeFilter.EndDate = this.DateTimeTo;

            this.UpdateFilter();
        }

        private void UpdateFilter()
        {
            this.OnFilterChanged();
        }

        #endregion

        public class DateTimeFilter : IFilter
        {
            #region Public Properties

            public DateTime? EndDate { get; set; }

            public DateTime? StartDate { get; set; }

            #endregion

            #region Public Methods and Operators

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (typeof(T) != typeof(TripViewModel))
                {
                    return items;
                }

                IEnumerable<TripViewModel> trips = items.Cast<TripViewModel>().ToList();
                return
                    trips.Where(
                        o =>
                        (!this.StartDate.HasValue || o.Trip.TripStartTime.Date >= this.StartDate.Value.Date)
                        && (!this.EndDate.HasValue || o.Trip.TripStartTime.Date <= this.EndDate.Value.Date)).Cast<T>();
            }

            #endregion
        }
    }
}
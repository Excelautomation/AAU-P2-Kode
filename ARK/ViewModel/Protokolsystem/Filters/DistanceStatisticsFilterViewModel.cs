using System;
using System.Collections.Generic;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class DistanceStatisticsFilterViewModel : FilterViewModelBase
    {
        private bool _statisticsAll;
        private bool _statisticsErgometer;
        private bool _statisticsKajak;
        private bool _statisticsInrigger;
        private bool _statisticsGig;
        private bool _statisticsOutrigger;

        public DistanceStatisticsFilterViewModel()
        {
            CurrentBoatType = new CategoryFilter<TripViewModel>(trip => true);

            StatisticsAll = true;

            UpdateFilter();
        }

        public CategoryFilter<TripViewModel> CurrentBoatType { get; set; }

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

        private void UpdateFilter()
        {
            base.OnFilterChanged();
        }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> {CurrentBoatType};
        }
    }
}
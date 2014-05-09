using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class TripsViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private Trip _currentTrip;
        private bool _recentSave;
        private List<Trip> _trips;
        private IEnumerable<Trip> _tripsFiltered;

        //private FrameworkElement _filter;

        public TripsViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                DbArkContext db = DbArkContext.GetDbContext();

                // Load data
                _trips = db.Trip
                    .Include(trip => trip.Members)
                    .ToList();

                // Reset filter
                ResetFilter();
            };

            //Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public IEnumerable<Trip> TripsFiltered
        {
            get { return _tripsFiltered; }
            set
            {
                _tripsFiltered = value;
                Notify();
            }
        }

        public Trip CurrentTrip
        {
            get { return _currentTrip; }
            set
            {
                _currentTrip = value;
                Notify();
            }
        }

        public bool RecentSave
        {
            get { return _recentSave; }
            set
            {
                _recentSave = value;
                Notify();
            }
        }

        public ICommand SelectedChange
        {
            get { return GetCommand<Trip>(e => { CurrentTrip = e; }); }
        }

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DbArkContext.GetDbContext().SaveChanges();
                    RecentSave = true;
                });
            }
        }

        public ICommand ShowBoatDialog
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DbArkContext.GetDbContext().SaveChanges();
                    RecentSave = true;
                });
            }
        }

        public FrameworkElement Filter
        {
            get { return new TripFilter(); }
        }

        private void ResetFilter()
        {
            TripsFiltered = _trips;
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
                TripsFiltered = FilterContent.FilterItems(TripsFiltered, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                TripsFiltered = TripsFiltered.Where(trip => trip.Filter(args.SearchEventArgs.SearchText)).ToList();
            }
        }
    }
}
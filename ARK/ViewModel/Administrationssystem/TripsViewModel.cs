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
using ARK.View.Administrationssystem.Pages;

namespace ARK.ViewModel.Administrationssystem
{
    public class TripsViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private Trip _currentTrip;
        private bool _recentSave = false;
        private List<Trip> _trips;
        private IEnumerable<Trip> _tripsFiltered;
        private List<Boat> _allBoats;
        public BoatListWindow NewBoatDialog;
        public DbArkContext db;

        
        //private FrameworkElement _filter;

        public TripsViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                db = DbArkContext.GetDbContext();

                // Load data
                _trips = db.Trip
                    .Include(trip => trip.Members)
                    .ToList();

                _allBoats = db.Boat.ToList();

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

        public List<Boat> AllBoats
        {
            get { return _allBoats; }
            set
            {
                _allBoats = value;
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
            get { return GetCommand<Trip>(e => 
            { 
                CurrentTrip = e;
                RecentSave = false;
            }); }
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
                    NewBoatDialog = new View.Administrationssystem.Pages.BoatListWindow();
                    NewBoatDialog.DataContext = this;
                    NewBoatDialog.ShowDialog();
                });
            }
        }

        public ICommand ShowBoatsContinue
        {
            get
            {
                return GetCommand<Boat>(e =>
                {
                    NewBoatDialog.Close();
                    CurrentTrip.Boat = e;
                    db.SaveChanges();
                    NotifyCustom("CurrentTrip");
                    NotifyCustom("TripsFiltered");

                    // Båd opdateres ikke i listview, men andre ændringer, som km sejlet gør. Meget mærkeligt.

                    //Trip TempTrip = CurrentTrip;
                    //TripsFiltered = db.Trip.ToList();
                    //CurrentTrip = TempTrip;
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
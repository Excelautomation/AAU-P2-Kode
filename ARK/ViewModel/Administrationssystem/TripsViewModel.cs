using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.View.Administrationssystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class TripsViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        public BoatListWindow NewBoatDialog;
        private List<Boat> _allBoats;
        private Trip _currentTrip;
        private bool _recentSave;
        private List<Trip> _trips;
        private IEnumerable<Trip> _tripsFiltered;

        public TripsViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                // Load data
                using (var db = new DbArkContext())
                {
                    _trips = db.Trip
                        .Include(trip => trip.Members)
                        .ToList();

                    _allBoats = db.Boat.ToList();

                    if (_trips.Count != 0)
                    {
                        CurrentTrip = _trips[0];
                        //LocalTrip = CurrentTrip;
                    }
                }        
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

        //public Trip LocalTrip
        //{
        //    get { return _localTrip; }
        //    set { _localTrip = value; Notify(); }
        //}

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
            get
            {
                return GetCommand(e =>
                {
                    CurrentTrip = (Trip)e;
                    //LocalTrip = CurrentTrip;
                    RecentSave = false;
                });
            }
        }

        public ICommand SaveChanges
        {
            get 
            {
                //CurrentTrip = LocalTrip;
                return GetCommand(Save); 
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                
                return GetCommand(Reload);
            }
        }

        public ICommand ShowBoatDialog
        {
            get
            {
                return GetCommand(() =>
                {
                    NewBoatDialog = new BoatListWindow();
                    NewBoatDialog.DataContext = this;
                    NewBoatDialog.ShowDialog();
                });
            }
        }

        public ICommand ShowBoatsContinue
        {
            get
            {
                return GetCommand(e =>
                {
                    NewBoatDialog.Close();
                    using (var db = new DbArkContext())
                    {
                        db.Trip.Attach(CurrentTrip);
                        CurrentTrip.Boat = (Boat)e;
                    }
                    NotifyCustom("CurrentTrip");
                });
            }
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(CurrentTrip).State = EntityState.Modified; // Får bådudskiftning i ture til at fucke op.
                db.SaveChanges();
            }
            // Trigger notify - reset lists
            UpdateList<Trip>(TripsFiltered);

            RecentSave = true;
        }

        public void UpdateList<T>(IEnumerable<T> list)
        {
            var tmp = TripsFiltered;
            TripsFiltered = null;
            TripsFiltered = tmp;
        }

        private void Reload()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(CurrentTrip).State = EntityState.Modified;
                db.Entry(CurrentTrip).Reload();
            }

            RecentSave = false;

            // Trigger notify - reset lists
            UpdateList<Trip>(TripsFiltered);

            NotifyCustom("CurrentTrip");
        }

        #region Filter

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

        #endregion
    }
}
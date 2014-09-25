using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private IEnumerable<Boat> _boats;

        private List<Boat> _boatsNonFiltered;

        private Boat _currentBoat;

        private FrameworkElement _filter;

        private Member _mostUsingMember;

        private bool _recentCancel;

        private bool _recentSave;

        private IEnumerable<Trip> _trips;

        private Trip _selectedTrip;

        public BoatViewModel()
        {
            ParentAttached += (sender, e) =>
                {
                    // Load data
                    using (var db = new DbArkContext())
                    {
                        _boatsNonFiltered =
                            db.Boat.Include(boat => boat.DamageForms).Include(boat => boat.Trips).ToList();

                        _trips = db.Trip.Include(trip => trip.Members).Include(trip => trip.Boat).ToList();
                    }

                    // Nulstil filter
                    ResetFilter();

                    // Sæt valgt båd
                    if (Boats.Count() != 0)
                    {
                        CurrentBoat = Boats.First();
                    }
                };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public IEnumerable<Boat> Boats
        {
            get
            {
                return _boats;
            }

            set
            {
                _boats = value;
                Notify();
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                return GetCommand(Reload);
            }
        }

        public Boat CurrentBoat
        {
            get
            {
                return _currentBoat;
            }

            set
            {
                if (_currentBoat != null)
                {
                    Reload();
                }

                _currentBoat = value;

                RecentSave = false;
                RecentCancel = false;
                Notify();
            }
        }

        public bool RecentCancel
        {
            get
            {
                return _recentCancel;
            }

            set
            {
                if (value != _recentCancel)
                {
                    _recentCancel = value;
                    _recentSave = false;
                    NotifyCustom("RecentSave");
                }

                Notify();
            }
        }

        public bool RecentSave
        {
            get
            {
                return _recentSave;
            }

            set
            {
                if (value != _recentSave)
                {
                    _recentSave = value;
                    _recentCancel = false;
                    NotifyCustom("RecentCancel");
                }

                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand(Save);
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return _filter ?? (_filter = new BoatsFilter());
            }
        }

        public void OpenBoat(Boat boat)
        {
            CurrentBoat = Boats.First(b => b.Id == boat.Id);
        }

        private void Reload()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(CurrentBoat).State = EntityState.Modified;
                db.Entry(CurrentBoat).Reload();
            }

            RecentCancel = true;

            // Trigger notify - reset lists
            IEnumerable<Boat> tmp = Boats;
            Boats = null;
            Boats = tmp;

            NotifyCustom("CurrentBoat");
        }

        private void ResetFilter()
        {
            Boats = _boatsNonFiltered.AsReadOnly();
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(CurrentBoat).State = EntityState.Modified;
                db.SaveChanges();
            }

            RecentSave = true;
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Tjek filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                Boats = FilterContent.FilterItems(Boats, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Boats = from boat in Boats where boat.Filter(args.SearchEventArgs.SearchText) select boat;
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;

                if (_selectedTrip != null)
                {
                    ShowTrip(_selectedTrip);
                }

                Notify();
            }
        }

        private void ShowTrip(Trip trip)
        {
            if (Parent == null)
                return;

            var adminSystem = (AdminSystemViewModel)Parent;

            adminSystem.MenuTrips.Execute(null);

            var tripViewModel = (TripsViewModel) adminSystem.CurrentPage.DataContext;
            var filterViewModel = (TripFilterViewModel) tripViewModel.Filter.DataContext;
            
            // Enable all filter
            EnableFilter(filterViewModel.ControlsBoatType);
            EnableFilter(filterViewModel.ControlsDistance);
            EnableFilter(filterViewModel.ControlsYear);

            tripViewModel.OpenTrip(trip);

            SelectedTrip = null;
        }

        private void EnableFilter(IEnumerable<CheckBox> list)
        {
            foreach (var l in list)
            {
                l.IsChecked = true;
            }
        }
    }
}
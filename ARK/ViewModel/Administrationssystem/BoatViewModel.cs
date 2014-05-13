using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using System;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private IEnumerable<Boat> _boats;
        private IEnumerable<Trip> _trips;
        private List<Boat> _boatsNonFiltered;
        private Boat _currentBoat;
        private Member _mostUsingMember;

        private FrameworkElement _filter;
        private bool _recentCancel;
        private bool _recentSave;

        public BoatViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                // Load data
                using (var db = new DbArkContext())
                {
                    _boatsNonFiltered = db.Boat
                        .Include(boat => boat.DamageForms)
                        .Include(boat => boat.Trips)
                        .ToList();

                    _trips = db.Trip
                        .Include(trip => trip.Members)
                        .Include(trip => trip.Boat)
                        .ToList();
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
            get { return _boats; }
            set
            {
                _boats = value;
                Notify();
            }
        }

        public Member MostUsingMember
        {
            get { return _mostUsingMember; }
            set { _mostUsingMember = value; Notify(); }
        }

        public void OpenBoat(Boat boat)
        {
            CurrentBoat = Boats.First(b => b.Id == boat.Id);
        }

        public Boat CurrentBoat
        {
            get { return _currentBoat; }
            set
            {
                if (_currentBoat != null)
                    Reload();

                _currentBoat = value;

                if (_currentBoat != null)
                    MostUsingMember = MostUsingMemberFunc();
                else
                    MostUsingMember = null;

                RecentSave = false;
                RecentCancel = false;
                Notify();
            }
        }

        #region State
        public bool RecentSave
        {
            get { return _recentSave; }
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

        public bool RecentCancel
        {
            get { return _recentCancel; }
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
        #endregion

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand<object>(e => Save());
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                return GetCommand<object>(e => Reload());
            }
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

        private void Reload()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(CurrentBoat).State = EntityState.Modified;
                db.Entry(CurrentBoat).Reload();
            }

            RecentCancel = true;

            // Trigger notify - reset lists
            var tmp = Boats;
            Boats = null;
            Boats = tmp;

            NotifyCustom("CurrentBoat");
        }

        #region Search

        private void ResetFilter()
        {
            Boats = _boatsNonFiltered.AsReadOnly();
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
                (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
                return;

            // Tjek filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                Boats = FilterContent.FilterItems(Boats, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Boats = from boat in Boats
                        where boat.Filter(args.SearchEventArgs.SearchText)
                        select boat;
            }
        }

        #endregion

        public FrameworkElement Filter
        {
            get { return _filter ?? (_filter = new BoatsFilter()); }
        }

        public Member MostUsingMemberFunc()
        {
            if (CurrentBoat.Trips.Count == 0)
                return null;
            else
            {
                var members = _trips
                    .Where(trip => trip.Boat == CurrentBoat);

                var m3 = CurrentBoat.Trips.SelectMany(trip => trip.Members);

                var o2 = m3.Select(member => CurrentBoat.Trips
                    .SelectMany(trip => trip.Members)
                    .Count(m => m == member));

                return (from member in CurrentBoat.Trips
                                       .SelectMany(trip => trip.Members)
                                       .Distinct()
                        orderby CurrentBoat.Trips
                                .SelectMany(trip => trip.Members)
                                .Count(m => m == member) descending
                        select member).First();
            }
        }
    }
}
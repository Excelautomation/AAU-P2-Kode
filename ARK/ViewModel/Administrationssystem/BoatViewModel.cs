using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using System.ComponentModel;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private List<Boat> _boatsNonFiltered;
        private readonly DbArkContext _dbArkContext;
        private bool _localActiveBoat;
        private IEnumerable<Boat> _boats;
        private bool _recentSave = false;
        private bool _recentCancel = false;
        private bool _recentInfoSave = false; // De tre sidste kan laves til enum
        private Boat _currentBoat;
        //private Member _MostUsingMember;

        private FrameworkElement _filter;

        public BoatViewModel()
        {
            // Load data
            _dbArkContext = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
            {
                DbArkContext db = DbArkContext.GetDbContext();

                // Load data
                _boatsNonFiltered = db.Boat.Include(boat => boat.DamageForms).Include(boat => boat.Trips).ToList();

            // Nulstil filter
            ResetFilter();

                // Sæt valgt båd
            if (Boats.Count() != 0)
            {
                CurrentBoat = Boats.First();
                LocalActiveBoat = CurrentBoat.Active;
            }
            };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

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

        public bool RecentInfoSave
        {
            get { return _recentInfoSave; }
            set
            {
                _recentInfoSave = value; Notify();
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

        public IEnumerable<Boat> Boats
        {
            get { return _boats; }
            set
            {
                _boats = value;
                Notify();
            }
        }

        public Boat CurrentBoat
        {
            get { return _currentBoat; }
            set
            {
                _currentBoat = value;
                if (value != null)
                    LocalActiveBoat = value.Active;
                RecentSave = false;
                RecentCancel = false;
                RecentInfoSave = false;
                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    if (CurrentBoat.Active != LocalActiveBoat) { 
                    CurrentBoat.Active = LocalActiveBoat;
                    Boat tempboat = CurrentBoat;
                    _dbArkContext.SaveChanges();
                    Boats = _dbArkContext.Boat.ToList();
                    CurrentBoat = tempboat;
                        RecentSave = true;
                    }
                });
            }
        }

        public ICommand SaveInfoChanges
        {
            get
            {
                return GetCommand<object>(e =>
                {
                        Boat tempboat = CurrentBoat;
                        _dbArkContext.SaveChanges();
                        Boats = _dbArkContext.Boat.ToList();
                        CurrentBoat = tempboat;
                        RecentInfoSave = true;
                });
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    if (CurrentBoat.Active != LocalActiveBoat)
                    {
                    LocalActiveBoat = CurrentBoat.Active;
                        RecentCancel = true;
                    }
                });
            }
        }

        public bool LocalActiveBoat
        {
            get { return _localActiveBoat; }
            set
            {
                _localActiveBoat = value;
                Notify();
            }
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
    }
}
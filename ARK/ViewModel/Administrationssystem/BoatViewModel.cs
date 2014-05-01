using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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
        private readonly List<Boat> _boatsNonFiltered;
        private readonly DbArkContext _dbArkContext;
        private bool _LocalActiveBoat;
        private IEnumerable<Boat> _boats;
        private bool _RecentSave = false;
        private bool _RecentCancel = false;
        private Boat _currentBoat;
        private FrameworkElement _filter;

        public BoatViewModel()
        {
            // Load data
            _dbArkContext = DbArkContext.GetDbContext();

            lock (_dbArkContext)
                _boatsNonFiltered = _dbArkContext.Boat.ToListAsync().Result;

            // Nulstil filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);

                // Sæt valgt båd
            if (Boats.Count() != 0)
            {
                CurrentBoat = Boats.First();
                LocalActiveBoat = CurrentBoat.Active;
            }

            
        }

        public bool RecentSave 
        { 
            get { return _RecentSave; }
            set 
            {
                if (value != _RecentSave)
                {
                    _RecentSave = value;
                    _RecentCancel = false;
                    Notify("RecentCancel");
                }
                Notify();
            }
        }
            
        public bool RecentCancel
        {
            get { return _RecentCancel; }
            set
            {
                if (value != _RecentCancel)
                {
                    _RecentCancel = value;
                    _RecentSave = false;
                    Notify("RecentSave");
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
                Notify();
            }
        }

        public ICommand SelectedChange
        {
            get 
            { 
                return GetCommand<Boat>(e => 
                { 
                    CurrentBoat = e;
                }); 
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
            get { return _LocalActiveBoat; }
            set
            {
                _LocalActiveBoat = value;
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
            if (!string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Boats = from boat in Boats
                    where boat.FilterBoat(args.SearchEventArgs.SearchText)
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
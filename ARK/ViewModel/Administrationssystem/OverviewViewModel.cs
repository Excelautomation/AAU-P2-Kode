using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class OverviewViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private IEnumerable<Boat> _boatsOut;
        private List<Boat> _boatsOutNonFiltered;
        private OverviewFilter _filter;
        private IEnumerable<LongTripForm> _longDistanceForms;
        private List<LongTripForm> _longDistanceFormsNonFiltered;
        private Visibility _showBoatsOut;
        private Visibility _showLangtur;
        private Visibility _showSkader;
        private IEnumerable<DamageForm> _skadesblanketter;
        private List<DamageForm> _skadesblanketterNonFiltered;

        public OverviewViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                DbArkContext db = DbArkContext.GetDbContext();

                // Load data
                _skadesblanketterNonFiltered = db.DamageForm
                    .Where(damageForm => !damageForm.Closed)
                    .OrderBy(damageForm => damageForm.Date)
                    .Take(6)
                    .ToList();

                _longDistanceFormsNonFiltered = db.LongTripForm
                    .Where(longDistanceForm => longDistanceForm.Status == LongTripForm.BoatStatus.Awaiting)
                    .OrderBy(longDistanceForm => longDistanceForm.PlannedStartDate)
                    .Take(6)
                    .ToList();

                _boatsOutNonFiltered = db.Boat
                    .Include(boat => boat.Trips)
                    .Where(boat => boat.Trips.Any(trip => trip.TripEndedTime == null))
                    .ToList()
                    .OrderBy(boat => boat.Trips.First(trip => trip.TripEndedTime == null).TripStartTime)
                    .ToList();

                // Nulstil filter
                ResetFilter();
            };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public IEnumerable<DamageForm> Skadesblanketter
        {
            get { return _skadesblanketter; }
            private set
            {
                _skadesblanketter = value;
                Notify();
            }
        }

        public IEnumerable<LongTripForm> LongDistanceForms
        {
            get { return _longDistanceForms; }
            private set
            {
                _longDistanceForms = value;
                Notify();
            }
        }

        public IEnumerable<Boat> BoatsOut
        {
            get { return _boatsOut; }
            private set
            {
                _boatsOut = value;
                Notify();
            }
        }

        public Visibility ShowBoatsOut
        {
            get { return _showBoatsOut; }
            set
            {
                _showBoatsOut = value;
                Notify();
            }
        }

        public Visibility ShowLangtur
        {
            get { return _showLangtur; }
            set
            {
                _showLangtur = value;
                Notify();
            }
        }

        public Visibility ShowSkader
        {
            get { return _showSkader; }
            set
            {
                _showSkader = value;
                Notify();
            }
        }

        #region Navigering via listitems
        private DamageForm _selectedDamageForm;
        public DamageForm SelectedDamageForm
        {
            get { return _selectedDamageForm; }
            set
            {
                _selectedDamageForm = value;

                if (_selectedDamageForm != null)
                    ShowDamageForm(_selectedDamageForm);

                Notify();
            }
        }

        private void ShowDamageForm(DamageForm SelectedDamageForm)
        {
            var adminSystem = (AdminSystemViewModel)Parent;
            adminSystem.MenuForms.Execute(null);
            var FormsViewModel = (FormsViewModel)adminSystem.CurrentPage.DataContext;

            FormsViewModel.SelectedTabIndex = 0;
            FormsViewModel.GoToDamageForm(SelectedDamageForm);
        }

        private LongTripForm _selectedLongDistanceForm;
        public LongTripForm SelectedLongDistanceForm
        {
            get { return _selectedLongDistanceForm; }
            set
            {
                _selectedLongDistanceForm = value;

                if (_selectedLongDistanceForm != null)
                    ShowLongDistanceForm(_selectedLongDistanceForm);

                Notify();
            }
        }

        private void ShowLongDistanceForm(LongTripForm LongDistanceForm)
        {
            var adminSystem = (AdminSystemViewModel)Parent;
            adminSystem.MenuForms.Execute(null);
            var FormsViewModel = (FormsViewModel)adminSystem.CurrentPage.DataContext;

            FormsViewModel.SelectedTabIndex = 1;
            FormsViewModel.GoToLongDistanceForm(LongDistanceForm);
        }

        private Boat _selectedBoat;
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;

                if (_selectedBoat != null)
                    ShowBoat(_selectedBoat);

                Notify();
            }
        }

        private void ShowBoat(Boat boat)
        {
            var adminSystem = (AdminSystemViewModel)Parent;
            adminSystem.MenuBoats.Execute(null);
            var boatsViewModel = (BoatViewModel)adminSystem.CurrentPage.DataContext;
            boatsViewModel.CurrentBoat = boat;
        }
        #endregion

        #region Seach and filters
        public FrameworkElement Filter
        {
            get
            {
                return _filter ?? (_filter = new OverviewFilter());
            }
        }

        private void ResetFilter()
        {
            ShowBoatsOut = Visibility.Visible;
            ShowLangtur = Visibility.Visible;
            ShowSkader = Visibility.Visible;

            Skadesblanketter = _skadesblanketterNonFiltered;
            LongDistanceForms = _longDistanceFormsNonFiltered;
            BoatsOut = _boatsOutNonFiltered;
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
                BoatsOut = FilterContent.FilterItems(BoatsOut, args.FilterEventArgs);
                LongDistanceForms = FilterContent.FilterItems(LongDistanceForms, args.FilterEventArgs);
                Skadesblanketter = FilterContent.FilterItems(Skadesblanketter, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Skadesblanketter = from skade in Skadesblanketter
                    where skade.Filter(args.SearchEventArgs.SearchText)
                    select skade;

                LongDistanceForms = from distanceform in LongDistanceForms
                    where distanceform.Filter(args.SearchEventArgs.SearchText)
                    select distanceform;

                BoatsOut = from boat in BoatsOut
                    where boat.Filter(args.SearchEventArgs.SearchText)
                    select boat;
            }

            // Skift visibility
            ShowSkader = Skadesblanketter.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            ShowLangtur = LongDistanceForms.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            ShowBoatsOut = BoatsOut.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        #endregion
    }
}
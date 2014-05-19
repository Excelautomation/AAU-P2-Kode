using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Administrationssystem.Filters;

namespace ARK.ViewModel.Administrationssystem
{
    public class OverviewViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        #region Fields

        private IEnumerable<Boat> _boatsOut;

        private List<Boat> _boatsOutNonFiltered;

        private OverviewFilter _filter;

        private bool _loadingData;

        private IEnumerable<LongTripForm> _longDistanceForms;

        private List<LongTripForm> _longDistanceFormsNonFiltered;

        private Boat _selectedBoat;

        private DamageForm _selectedDamageForm;

        private LongTripForm _selectedLongDistanceForm;

        private Visibility _showBoatsOut;

        private Visibility _showLangtur;

        private Visibility _showSkader;

        private IEnumerable<DamageForm> _skadesblanketter;

        private List<DamageForm> _skadesblanketterNonFiltered;

        #endregion

        #region Constructors and Destructors

        public OverviewViewModel()
        {
            // Initilize lists
            this._skadesblanketterNonFiltered = new List<DamageForm>();
            this._longDistanceFormsNonFiltered = new List<LongTripForm>();
            this._boatsOutNonFiltered = new List<Boat>();

            this.ParentAttached += (sender, e) => Task.Factory.StartNew(
                () =>
                    {
                        this.LoadingData = true;

                        // Load data
                        using (var db = new DbArkContext())
                        {
                            this._skadesblanketterNonFiltered =
                                db.DamageForm.Where(damageForm => !damageForm.Closed)
                                    .OrderBy(damageForm => damageForm.Date)
                                    .Include(damageForm => damageForm.Boat)
                                    .Include(damageForm => damageForm.RegisteringMember)
                                    .Take(6)
                                    .ToList();

                            this._longDistanceFormsNonFiltered =
                                db.LongTripForm.Where(
                                    longDistanceForm => longDistanceForm.Status == LongTripForm.BoatStatus.Awaiting)
                                    .OrderBy(longDistanceForm => longDistanceForm.PlannedStartDate)
                                    .Include(longDistanceForm => longDistanceForm.Boat)
                                    .Include(longDistanceForm => longDistanceForm.ResponsibleMember)
                                    .Take(6)
                                    .ToList();

                            this._boatsOutNonFiltered =
                                db.Boat.Include(boat => boat.Trips)
                                    .Where(boat => boat.Trips.Any(trip => trip.TripEndedTime == null))
                                    .ToList()
                                    .OrderBy(boat => boat.Trips.First(trip => trip.TripEndedTime == null).TripStartTime)
                                    .ToList();

                            // Nulstil filter
                            this.ResetFilter();
                            this.LoadingData = false;
                        }
                    }).Wait(500);

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => this.UpdateFilter(eventArgs);
        }

        #endregion

        #region Public Properties

        public IEnumerable<Boat> BoatsOut
        {
            get
            {
                return this._boatsOut;
            }

            private set
            {
                this._boatsOut = value;
                this.Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return this._filter ?? (this._filter = new OverviewFilter());
            }
        }

        public bool LoadingData
        {
            get
            {
                return this._loadingData;
            }

            set
            {
                this._loadingData = value;
                this.Notify();
            }
        }

        public IEnumerable<LongTripForm> LongDistanceForms
        {
            get
            {
                return this._longDistanceForms;
            }

            private set
            {
                this._longDistanceForms = value;
                this.Notify();
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return this._selectedBoat;
            }

            set
            {
                this._selectedBoat = value;

                if (this._selectedBoat != null)
                {
                    this.ShowBoat(this._selectedBoat);
                }

                this.Notify();
            }
        }

        public DamageForm SelectedDamageForm
        {
            get
            {
                return this._selectedDamageForm;
            }

            set
            {
                this._selectedDamageForm = value;

                if (this._selectedDamageForm != null)
                {
                    this.ShowDamageForm(this._selectedDamageForm);
                }

                this.Notify();
            }
        }

        public LongTripForm SelectedLongDistanceForm
        {
            get
            {
                return this._selectedLongDistanceForm;
            }

            set
            {
                this._selectedLongDistanceForm = value;

                if (this._selectedLongDistanceForm != null)
                {
                    this.ShowLongDistanceForm(this._selectedLongDistanceForm);
                }

                this.Notify();
            }
        }

        public Visibility ShowBoatsOut
        {
            get
            {
                return this._showBoatsOut;
            }

            set
            {
                this._showBoatsOut = value;
                this.Notify();
            }
        }

        public Visibility ShowLangtur
        {
            get
            {
                return this._showLangtur;
            }

            set
            {
                this._showLangtur = value;
                this.Notify();
            }
        }

        public Visibility ShowSkader
        {
            get
            {
                return this._showSkader;
            }

            set
            {
                this._showSkader = value;
                this.Notify();
            }
        }

        public IEnumerable<DamageForm> Skadesblanketter
        {
            get
            {
                return this._skadesblanketter;
            }

            private set
            {
                this._skadesblanketter = value;
                this.Notify();
            }
        }

        #endregion

        #region Methods

        public void ResetFilter()
        {
            this.ShowBoatsOut = Visibility.Visible;
            this.ShowLangtur = Visibility.Visible;
            this.ShowSkader = Visibility.Visible;

            this.Skadesblanketter = this._skadesblanketterNonFiltered;
            this.LongDistanceForms = this._longDistanceFormsNonFiltered;
            this.BoatsOut = this._boatsOutNonFiltered;
        }

        private void ShowBoat(Boat boat)
        {
            var adminSystem = (AdminSystemViewModel)this.Parent;
            adminSystem.MenuBoats.Execute(null);
            var boatsViewModel = (BoatViewModel)adminSystem.CurrentPage.DataContext;
            boatsViewModel.OpenBoat(boat);
        }

        private void ShowDamageForm(DamageForm damageForm)
        {
            var adminSystem = (AdminSystemViewModel)this.Parent;
            adminSystem.MenuForms.Execute(null);
            var formsViewModel = (FormsViewModel)adminSystem.CurrentPage.DataContext;
            var filterViewModel = (FormsFilterViewModel)formsViewModel.Filter.DataContext;
            filterViewModel.ShowOpen = true;

            formsViewModel.SelectedTabIndex = 0;
            formsViewModel.OpenDamageForm(damageForm);
        }

        private void ShowLongDistanceForm(LongTripForm longDistanceForm)
        {
            var adminSystem = (AdminSystemViewModel)this.Parent;
            adminSystem.MenuForms.Execute(null);

            var formsViewModel = (FormsViewModel)adminSystem.CurrentPage.DataContext;
            var filterViewModel = (FormsFilterViewModel)formsViewModel.Filter.DataContext;

            filterViewModel.ShowOpen = true;
            formsViewModel.OpenLongDistanceForm(longDistanceForm);
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            this.ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.BoatsOut = FilterContent.FilterItems(this.BoatsOut, args.FilterEventArgs);
                this.LongDistanceForms = FilterContent.FilterItems(this.LongDistanceForms, args.FilterEventArgs);
                this.Skadesblanketter = FilterContent.FilterItems(this.Skadesblanketter, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.Skadesblanketter = from skade in this.Skadesblanketter
                                        where skade.Filter(args.SearchEventArgs.SearchText)
                                        select skade;

                this.LongDistanceForms = from distanceform in this.LongDistanceForms
                                         where distanceform.Filter(args.SearchEventArgs.SearchText)
                                         select distanceform;

                this.BoatsOut = from boat in this.BoatsOut
                                where boat.Filter(args.SearchEventArgs.SearchText)
                                select boat;
            }

            // Skift visibility
            this.ShowSkader = this.Skadesblanketter.Any() ? Visibility.Visible : Visibility.Collapsed;

            this.ShowLangtur = this.LongDistanceForms.Any() ? Visibility.Visible : Visibility.Collapsed;

            this.ShowBoatsOut = this.BoatsOut.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }
}
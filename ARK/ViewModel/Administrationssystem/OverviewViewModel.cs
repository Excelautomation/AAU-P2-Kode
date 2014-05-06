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

namespace ARK.ViewModel.Administrationssystem
{
    public class OverviewViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        private IEnumerable<Boat> _boatsOut;
        private List<Boat> _boatsOutNonFiltered;
        private OverviewFilter _filter;
        private IEnumerable<LongDistanceForm> _longDistanceForms;
        private List<LongDistanceForm> _longDistanceFormsNonFiltered;
        private Visibility _showBoatsOut;
        private Visibility _showLangtur;
        private Visibility _showSkader;
        private IEnumerable<DamageForm> _skadesblanketter;
        private List<DamageForm> _skadesblanketterNonFiltered;

        public OverviewViewModel()
        {
            // Instaliser lister så lazy ikke fejler
            _skadesblanketterNonFiltered = new List<DamageForm>();
            _longDistanceFormsNonFiltered = new List<LongDistanceForm>();
            _boatsOutNonFiltered = new List<Boat>();

            // Load data
            Task.Factory.StartNew(() =>
            {
                DbArkContext db = DbArkContext.GetDbContext();

                lock (db)
                {
                    // Opret forbindelser Async
                    Task<List<DamageForm>> damageforms = db.DamageForm.ToListAsync();
                    Task<List<LongDistanceForm>> longDistanceForms = db.LongTripForm.ToListAsync();
                    Task<List<Boat>> boatsOut = db.Boat.ToListAsync();

                    _skadesblanketterNonFiltered = damageforms.Result;
                    _longDistanceFormsNonFiltered = longDistanceForms.Result;
                    _boatsOutNonFiltered = boatsOut.Result;
                }

                // Nulstil filter
                ResetFilter();
            });

            // Nulstil filter
            ResetFilter();

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

        public IEnumerable<LongDistanceForm> LongDistanceForms
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

            Skadesblanketter = _skadesblanketterNonFiltered.AsReadOnly();
            LongDistanceForms = _longDistanceFormsNonFiltered.AsReadOnly();
            BoatsOut = _boatsOutNonFiltered.AsReadOnly();
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using CheckBox = System.Windows.Controls.CheckBox;

namespace ARK.ViewModel.Administrationssystem
{
    public class OverviewViewModel : ContentViewModelBase
    {
        private IEnumerable<Boat> _boatsOut;
        private List<Boat> _boatsOutNonFiltered;
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
                var db = DbArkContext.GetDbContext();

                _skadesblanketterNonFiltered = db.DamageForm.ToList();
                _longDistanceFormsNonFiltered = db.LongTripForm.ToList();
                _boatsOutNonFiltered = db.Boat.ToList();

                // Nulstil filter
                ResetFilter();
            });

            // Nulstil filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true, Filters());
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

        private void ResetFilter()
        {
            ShowBoatsOut = Visibility.Visible;
            ShowLangtur = Visibility.Visible;
            ShowSkader = Visibility.Visible;

            Skadesblanketter = _skadesblanketterNonFiltered.AsReadOnly();
            LongDistanceForms = _longDistanceFormsNonFiltered.AsReadOnly();
            BoatsOut = _boatsOutNonFiltered.AsReadOnly();
            //BoatsOut =
            //    from boat in _boatsOutNonFiltered.AsReadOnly()
            //    where boat.BoatOut == true
            //    orderby boat.Name ascending
            //    select boat;
        }

        private void UpdateFilter(FilterEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if (!args.Filters.Any() && string.IsNullOrEmpty(args.SearchText))
                return;

            // Tjek filter
            if (args.Filters.Any())
            {
                if (!args.Filters.Any(c => c == "Både ude"))
                     BoatsOut = new List<Boat>();

                if (!args.Filters.Any(c => c == "Langtur"))
                    LongDistanceForms = new List<LongDistanceForm>();

                if (!args.Filters.Any(c => c == "Skader"))
                    Skadesblanketter = new List<DamageForm>();
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(args.SearchText))
            {
                Skadesblanketter = from skade in Skadesblanketter
                                   where skade.FilterDamageForms(args.SearchText)
                    select skade;

                LongDistanceForms = from distanceform in LongDistanceForms
                    where distanceform.FilterLongDistanceForm(args.SearchText)
                    select distanceform;

                BoatsOut = from boat in BoatsOut
                    where boat.FilterBoat(args.SearchText)
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

        private IEnumerable<FrameworkElement> Filters()
        {
            return new ObservableCollection<FrameworkElement>
            {
                new CheckBox {Content = "Skader"},
                new CheckBox {Content = "Langtur"},
                new CheckBox {Content = "Både ude"}
            };
        }
    }
}
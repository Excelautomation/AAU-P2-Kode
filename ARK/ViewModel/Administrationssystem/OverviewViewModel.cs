using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class OverviewViewModel : FilterContentViewModelBase
    {
        private Visibility _showBoatsOut;
        private Visibility _showLangtur;
        private Visibility _showSkader;

        private IEnumerable<DamageForm> _skadesblanketter;
        private readonly List<DamageForm> _skadesblanketterNonFiltered;
        private IEnumerable<LongDistanceForm> _longDistanceForms;
        private readonly List<LongDistanceForm> _longDistanceFormsNonFiltered;
        private IEnumerable<Boat> _boatsOut;
        private readonly List<Boat> _boatsOutNonFiltered; 

        public OverviewViewModel()
        {
            // Load data
            using (var db = new DbArkContext())
            {
                _skadesblanketterNonFiltered = db.DamageForm.ToList();
                _longDistanceFormsNonFiltered = db.LongTripForm.ToList();
                _boatsOutNonFiltered = db.Boat.ToList();
            }

            // Aktiver filtre
            ParentAttached += (sender, args) =>
            {
                FilterContainer.EnableSearch = true;
                FilterContainer.EnableFilters = true;

                // Opdater filtre
                FilterContainer.Filters.Clear();

                FilterContainer.Filters.Add(new CheckBox {Content = "Skader"});
                FilterContainer.Filters.Add(new CheckBox {Content = "Langtur"});
                FilterContainer.Filters.Add(new CheckBox {Content = "Både ude"});

                // Opret checkbox filters
                CheckboxFilters = FilterContainer.Filters.Select(element => new CheckboxFilter(element as CheckBox, UpdateFilter)).ToList();

                // Bind til søgeevent
                FilterContainer.SearchTextChanged += (o, eventArgs) =>
                {
                    SearchText = eventArgs.SearchText;
                    UpdateFilter();
                };

                // Opdater filter
                UpdateFilter();
            };
        }

        private List<CheckboxFilter> CheckboxFilters { get; set; }
        private string SearchText { get; set; }

        public IEnumerable<DamageForm> Skadesblanketter 
        { 
            get { return _skadesblanketter; } 
            private set { _skadesblanketter = value; Notify(); } 
        }

        public IEnumerable<LongDistanceForm> LongDistanceForms
        {
            get { return _longDistanceForms; }
            private set { _longDistanceForms = value; Notify(); }
        }

        public IEnumerable<Boat> BoatsOut { get { return _boatsOut; } private set { _boatsOut = value; Notify(); } }

        public Visibility ShowBoatsOut
        {
            get { return _showBoatsOut; }
            set { _showBoatsOut = value; Notify(); }
        }

        public Visibility ShowLangtur
        {
            get { return _showLangtur; }
            set { _showLangtur = value; Notify(); }
        }

        public Visibility ShowSkader
        {
            get { return _showSkader; }
            set { _showSkader = value; Notify(); }
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

        private void UpdateFilter()
        {
            // Nulstil filter
            ResetFilter();

            // Indlæs de valgte checkbox filtre
            var selectedCheckboxFilters = (from filter in CheckboxFilters
                where filter.Active
                select filter).ToList();

            // Tjek om en af filtertyperne er aktive
            if (!selectedCheckboxFilters.Any() && string.IsNullOrEmpty(SearchText))
                return;


            // Tjek filter
            if (selectedCheckboxFilters.Any())
            {
                ShowBoatsOut = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Både ude")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                ShowLangtur = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Langtur")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                ShowSkader = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Skader")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(SearchText))
            {
                Skadesblanketter = from skade in Skadesblanketter
                    where skade.Boat.Name.Contains(SearchText) ||
                          skade.Description.Contains(SearchText)
                    select skade;

                ShowSkader = Skadesblanketter.Any() ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : FilterContentViewModelBase, IDisposable
    {
        private readonly List<Boat> _boatsNonFiltered;
        private bool _activeBoat;
        private Boat _currentBoat;
        private IEnumerable<Boat> _boats;
        private DbArkContext _dbArkContext;

        const string bådeUdeText = "Både ude";
        const string bådeHjemmeText = "Både hjemme";
        const string bådeUnderReparationText = "Både under reparation";
        const string beskadigedeBådeText = "Beskadigede både";
        const string inaktiveBådeText = "Inaktive både";
        const string funktionelleBådeText = "Funktionelle både";

        public BoatViewModel()
        {
            // Load data
            _dbArkContext = new DbArkContext();
            _boatsNonFiltered = _dbArkContext.Boat.ToList();

            // Aktiver filtre
            #region filtre
            ParentAttached += (sender, args) =>
            {
                FilterContainer.EnableSearch = true;
                FilterContainer.EnableFilters = true;

                // Opdater filtre
                FilterContainer.Filters.Clear();

                FilterContainer.Filters.Add(new CheckBox { Content = bådeUdeText });
                FilterContainer.Filters.Add(new CheckBox { Content = bådeHjemmeText });

                FilterContainer.Filters.Add(new Separator { Height = 20 });
                
                FilterContainer.Filters.Add(new CheckBox { Content = bådeUnderReparationText });
                FilterContainer.Filters.Add(new CheckBox { Content = beskadigedeBådeText });
                FilterContainer.Filters.Add(new CheckBox { Content = inaktiveBådeText });
                FilterContainer.Filters.Add(new CheckBox { Content = funktionelleBådeText });

                // Opret checkbox filters
                CheckboxFilters =
                    FilterContainer.Filters.Where(e => e is CheckBox)
                        .Select(element => new CheckboxFilter(element as CheckBox, UpdateFilter))
                        .ToList();

                // Bind til søgeevent
                FilterContainer.SearchTextChanged += (o, eventArgs) =>
                {
                    SearchText = eventArgs.SearchText;
                    UpdateFilter();
                };

                // Opdater filter
                UpdateFilter();
            #endregion

            // Sæt valgt båd
            if (Boats.Count() != 0)
            {
                CurrentBoat = Boats.First();
                _activeBoat = CurrentBoat.Active;
            }
            };
        }

        private List<CheckboxFilter> CheckboxFilters { get; set; }
        private string SearchText { get; set; }

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
                //if (_currentBoat != null)
                //  _activeBoat = _currentBoat.Active;
                Notify();
            }
        }

        public ICommand SelectedChange
        {
            get 
            { 
                return GetCommand<Boat>(e => { CurrentBoat = e; }); 
            }
            
        }

        public int ActiveBoat
        {
            get { if (_activeBoat) return 0; else return 1; }
            set
            {
                if (value == 0) _activeBoat = true;
                else _activeBoat = false;
                Notify();
            }
        }


        #region Search
        private void ResetFilter()
        {
            Boats = _boatsNonFiltered.AsReadOnly();
        }

        private void UpdateFilter()
        {
            // Nulstil filter
            ResetFilter();

            // Indlæs de valgte checkbox filtre
            List<CheckboxFilter> selectedCheckboxFilters = (from filter in CheckboxFilters
                                                            where filter.Active
                                                            select filter).ToList();

            // Tjek om en af filtertyperne er aktive
            if (!selectedCheckboxFilters.Any() && string.IsNullOrEmpty(SearchText))
                return;

            // Bool variablel der husker på om listen er blevet opdateret
            bool listUpdated = false;

            // Tjek filter
            if (selectedCheckboxFilters.Any())
            {
                if (FilterActive(bådeUdeText, selectedCheckboxFilters))
                {
                    Boats = from boat in _boatsNonFiltered
                                        where boat.BoatOut
                                        select boat;

                    listUpdated = true;
                }

                if (FilterActive(bådeHjemmeText, selectedCheckboxFilters))
                {
                    var output = from boat in _boatsNonFiltered
                                                           where boat.BoatOut == false
                                                           select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (FilterActive(bådeUnderReparationText, selectedCheckboxFilters))
                {
                    var output = from boat in _boatsNonFiltered
                                 where !boat.Usable
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (FilterActive(beskadigedeBådeText, selectedCheckboxFilters))
                {
                    var output = from boat in _boatsNonFiltered
                                 where boat.Active
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (FilterActive(inaktiveBådeText, selectedCheckboxFilters))
                {
                    var output = from boat in _boatsNonFiltered
                                 where !boat.Active
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (FilterActive(funktionelleBådeText, selectedCheckboxFilters))
                {
                    var output = from boat in _boatsNonFiltered
                                 where boat.Usable
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(SearchText))
            {
                Boats = from boat in Boats
                    where boat.FilterBoat(SearchText)
                    select boat;
            }
        }

        private void UpdateBoatsFilter(ref bool listUpdated, IEnumerable<Boat> output)
        {
            if (!listUpdated)
            {
                Boats = output;

                listUpdated = true;
            }
            else
            {
                Boats = MergeLists(Boats, output);
            }
        }

        private bool FilterActive(string filter, List<CheckboxFilter> selectedCheckboxFilters)
        {
            return selectedCheckboxFilters.Any(c => (string) c.Control.Content == filter);
        }

        private IEnumerable<T> MergeLists<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var outputList = new List<T>(list1);

            foreach (T elm in list2)
                if (!outputList.Any(e => e.Equals(elm)))
                    outputList.Add(elm);

            return outputList;
        }
        #endregion

        public void Dispose()
        {
            _dbArkContext.Dispose();
        }
    }
}
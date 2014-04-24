using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : FilterContentViewModelBase
    {
        private readonly List<Boat> _boatsNonFiltered;
        private bool _activeBoat;
        private Boat _currentBoat;
        private IEnumerable<Boat> _boats;
        private ObservableCollection<Control> _filters;

        const string bådeUdeText = "Både ude";
        const string bådeHjemmeText = "Både hjemme";
        const string bådeUnderReparationText = "Både under reparation";
        const string beskadigedeBådeText = "Beskadigede både";
        const string inaktiveBådeText = "Inaktive både";
        const string funktionelleBådeText = "Funktionelle både";

        public BoatViewModel()
        {
            // Load data
            using (var db = new DbArkContext())
            {
                _boatsNonFiltered = new List<Boat>(db.Boat).Where(x => x.Active).OrderBy(x => x.NumberofSeats).ToList();
            }

            // Aktiver filtre
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

                // Sæt valgt båd
                if (Boats.Count() != 0)
                {
                    CurrentBoat = Boats.First();
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
                Notify();
            }
        }

        public ICommand SelectedChange
        {
            get { return GetCommand<Boat>(e => { CurrentBoat = e; }); }
        }

        public bool ActiveBoat
        {
            get { return _activeBoat; }
            set
            {
                _activeBoat = value;
                Notify();
            }
        }

        public bool StringToBool(string str)
        {
            if (str.ToLower() == "ja" || str.ToLower() == "true")
                return true;
            if (str.ToLower() == "nej" || str.ToLower() == "false")
                return false;
            throw new ArgumentNullException();
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
                // TODO: Problem ved valg af kun bla/bla

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
                    throw new NotImplementedException();

                    var output = from boat in _boatsNonFiltered
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
                throw new NotImplementedException();
                
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
    }
}
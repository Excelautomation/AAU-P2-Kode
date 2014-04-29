using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ContentViewModelBase, IDisposable
    {
        private const string bådeUdeText = "Både ude";
        private const string bådeHjemmeText = "Både hjemme";
        private const string bådeUnderReparationText = "Både under reparation";
        private const string beskadigedeBådeText = "Beskadigede både";
        private const string inaktiveBådeText = "Inaktive både";
        private const string funktionelleBådeText = "Funktionelle både";
        private readonly List<Boat> _boatsNonFiltered;
        private readonly DbArkContext _dbArkContext;
        private bool _LocalActiveBoat;
        private IEnumerable<Boat> _boats;
        private Boat _currentBoat;

        public BoatViewModel()
        {
            // Load data
            _dbArkContext = new DbArkContext();
            _boatsNonFiltered = _dbArkContext.Boat.ToList();

            // Nulstil filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true, Filters());
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);

            // Sæt valgt båd
            if (Boats.Count() != 0)
            {
                CurrentBoat = Boats.First();
                LocalActiveBoat = CurrentBoat.Active;
            }
        }

        public void Dispose()
        {
            _dbArkContext.Dispose();
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
                LocalActiveBoat = value.Active;
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
                    CurrentBoat.Active = LocalActiveBoat;
                    Boat tempboat = CurrentBoat;
                    _dbArkContext.SaveChanges();
                    Boats = _dbArkContext.Boat.ToList();
                    CurrentBoat = tempboat;
                    System.Windows.MessageBox.Show("Ændringer gemt.");
                });
            }
            }
            
        public ICommand CancelChanges
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LocalActiveBoat = CurrentBoat.Active;
                    System.Windows.MessageBox.Show("Ændringer annulleret");
                    // messagebox der siger "Ændringer er annulleret."
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

        private void UpdateFilter(FilterEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if (!args.Filters.Any() && string.IsNullOrEmpty(args.SearchText))
                return;

            // Bool variablel der husker på om listen er blevet opdateret
            bool listUpdated = false;

            // Tjek filter
            if (args.Filters.Any())
            {
                if (args.Filters.All(c => c != bådeUdeText))
                {
                    Boats = from boat in _boatsNonFiltered
                                        where boat.BoatOut
                                        select boat;

                    listUpdated = true;
                }

                if (args.Filters.All(c => c != bådeHjemmeText))
                {
                    IEnumerable<Boat> output = from boat in _boatsNonFiltered
                                                           where boat.BoatOut == false
                                                           select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (args.Filters.All(c => c != bådeUnderReparationText))
                {
                    IEnumerable<Boat> output = from boat in _boatsNonFiltered
                                 where !boat.Usable
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (args.Filters.All(c => c != beskadigedeBådeText))
                {
                    IEnumerable<Boat> output = from boat in _boatsNonFiltered
                                 where boat.Active
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (args.Filters.All(c => c != inaktiveBådeText))
                {
                    IEnumerable<Boat> output = from boat in _boatsNonFiltered
                                 where !boat.Active
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }

                if (args.Filters.All(c => c != funktionelleBådeText))
                {
                    IEnumerable<Boat> output = from boat in _boatsNonFiltered
                                 where boat.Usable
                                 select boat;
                    UpdateBoatsFilter(ref listUpdated, output);
                }
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(args.SearchText))
            {
                Boats = from boat in Boats
                    where boat.FilterBoat(args.SearchText)
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
                Boats = FilterContent.MergeLists(Boats, output);
            }
        }

        private ObservableCollection<FrameworkElement> Filters()
        {
            return new ObservableCollection<FrameworkElement>
        {
                new CheckBox {Content = bådeUdeText},
                new CheckBox {Content = bådeHjemmeText},
                new Separator {Height = 20},
                new CheckBox {Content = bådeUnderReparationText},
                new CheckBox {Content = beskadigedeBådeText},
                new CheckBox {Content = inaktiveBådeText},
                new CheckBox {Content = funktionelleBådeText}
            };
        }

        #endregion

        
    }
}
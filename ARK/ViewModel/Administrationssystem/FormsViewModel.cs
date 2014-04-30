using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsViewModel : PageContainerViewModelBase, IContentViewModelBase, IDisposable
    {
        private readonly List<DamageForm> _damageFormsNonFiltered;
        private readonly List<LongDistanceForm> _longTripFormsNonFiltered;
        private IEnumerable<DamageForm> _damageForms;
        private IEnumerable<LongDistanceForm> _longTripForms;
        private Visibility _showDamageForms;
        private Visibility _showLongTripForms;

        private DbArkContext _dbArkContext;

        public FormsViewModel()
        {
            _dbArkContext = DbArkContext.GetDbContext();

            // Indlæs data
            _damageFormsNonFiltered = _dbArkContext.DamageForm.ToList();
            _longTripFormsNonFiltered = _dbArkContext.LongTripForm.ToList();

            // Nulstil filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true, Filters());
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public void Dispose()
        {
            _dbArkContext.Dispose();
        }

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return
                    GetCommand<DamageForm>(
                        e =>
                        {
                            NavigateToPage(new Lazy<FrameworkElement>(() => new FormsDamage()),
                                e.Description);

                            // Sæt damageform
                            var vm = CurrentPage.DataContext as FormsDamageViewModel;
                            if (vm != null)
                                vm.DamageForm = e;
                        });
            }
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return
                    GetCommand<LongDistanceForm>(
                        e =>
                        {
                            NavigateToPage(new Lazy<FrameworkElement>(() => new FormsLongTrip()),
                                e.Text);

                            var vm = CurrentPage.DataContext as FormsLongTripViewModel;
                            if (vm != null)
                                vm.LongDistanceForm = e;
                        });
            }
        }

        #region Filters

        public IFilterContainerViewModel FilterContainer
        {
            get { return Parent as IFilterContainerViewModel; }
        }

        public IEnumerable<DamageForm> DamageForms
        {
            get { return _damageForms; }
            private set
            {
                _damageForms = value;
                Notify();
            }
        }

        public IEnumerable<LongDistanceForm> LongDistanceForms
        {
            get { return _longTripForms; }
            private set
            {
                _longTripForms = value;
                Notify();
            }
        }

        public Visibility ShowDamageForms
        {
            get { return _showDamageForms; }
            set
            {
                _showDamageForms = value;
                Notify();
            }
        }

        public Visibility ShowLongDistanceForms
        {
            get { return _showLongTripForms; }
            set
            {
                _showLongTripForms = value;
                Notify();
            }
        }

        private void ResetFilter()
        {
            ShowDamageForms = Visibility.Visible;
            ShowLongDistanceForms = Visibility.Visible;

            DamageForms = _damageFormsNonFiltered.AsReadOnly();
            LongDistanceForms = _longTripFormsNonFiltered.AsReadOnly();
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
                ShowDamageForms = args.Filters.Any(c => c == "Skader")
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                ShowLongDistanceForms = args.Filters.Any(c => c == "Langtur")
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                // TODO: Problem ved valg af kun godkendte/afviste

                if (args.Filters.Any(c => c == "Afviste"))
                {
                    LongDistanceForms = from form in _longTripFormsNonFiltered
                        where form.Approved == false
                        select form;

                    listUpdated = true;
                }

                if (args.Filters.Any(c => c == "Godkendte"))
                {
                    IEnumerable<LongDistanceForm> output = from form in _longTripFormsNonFiltered
                        where form.Approved == false
                        select form;
                    if (!listUpdated)
                    {
                        LongDistanceForms = output;

                        listUpdated = true;
                    }
                    else
                    {
                        LongDistanceForms = FilterContent.MergeLists(LongDistanceForms, output);
                    }
                }
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(args.SearchText))
            {
                DamageForms = from damage in DamageForms
                              where damage.FilterDamageForms(args.SearchText)
                                   select damage;

                LongDistanceForms = from form in LongDistanceForms
                                    where form.FilterLongDistanceForm(args.SearchText)
                    select form;

                ShowDamageForms = DamageForms.Any() 
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                ShowLongDistanceForms = LongDistanceForms.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private IEnumerable<FrameworkElement> Filters()
        {
            return new ObservableCollection<FrameworkElement>
        {
                new CheckBox {Content = "Langtur"},
                new CheckBox {Content = "Skader"},
                new Separator {Height = 20},
                new CheckBox {Content = "Afviste"},
                new CheckBox {Content = "Godkendte"}
            };
        }

        #endregion

        #region IContentViewModelBase

        private IViewModelBase _parent;

        public IViewModelBase Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                if (ParentAttached != null) ParentAttached(this, new EventArgs());
            }
        }

        public event EventHandler ParentAttached;

        #endregion

        
    }
}
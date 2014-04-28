using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsViewModel : PageContainerViewModelBase, IContentViewModelBase
    {
        private readonly List<DamageForm> _damageFormsNonFiltered;
        private readonly List<LongDistanceForm> _longTripFormsNonFiltered;
        private IEnumerable<DamageForm> _damageForms;
        private IEnumerable<LongDistanceForm> _longTripForms;
        private Visibility _showDamageForms;
        private Visibility _showLongTripForms;

        public FormsViewModel()
        {
            using (var db = new DbArkContext())
            {
                _damageFormsNonFiltered = db.DamageForm.ToList();
                _longTripFormsNonFiltered = db.LongTripForm.ToList();
            }

            // Aktiver filtre
            ParentAttached += (sender, args) =>
            {
                FilterContainer.EnableSearch = true;
                FilterContainer.EnableFilters = true;

                // Opdater filtre
                FilterContainer.Filters.Clear();

                FilterContainer.Filters.Add(new CheckBox {Content = "Langtur"});
                FilterContainer.Filters.Add(new CheckBox {Content = "Skader"});
                FilterContainer.Filters.Add(new Separator {Height = 20});
                FilterContainer.Filters.Add(new CheckBox {Content = "Afviste"});
                FilterContainer.Filters.Add(new CheckBox {Content = "Godkendte"});

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
            };
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

        private List<CheckboxFilter> CheckboxFilters { get; set; }
        private string SearchText { get; set; }

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
                ShowDamageForms = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Skader")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                ShowLongDistanceForms = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Langtur")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                // TODO: Problem ved valg af kun godkendte/afviste

                if (selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Afviste"))
                {
                    LongDistanceForms = from form in _longTripFormsNonFiltered
                        where form.Approved == false
                        select form;

                    listUpdated = true;
                }

                if (selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Godkendte"))
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
                        LongDistanceForms = MergeLists(LongDistanceForms, output);
                    }
                }
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(SearchText))
            {
                DamageForms = from damage in DamageForms
                                   where damage.FilterDamageForms(SearchText)
                                   select damage;

                LongDistanceForms = from form in LongDistanceForms
                    where form.FilterLongDistanceForm(SearchText)
                    select form;

                ShowDamageForms = DamageForms.Any() 
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                ShowLongDistanceForms = LongDistanceForms.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
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
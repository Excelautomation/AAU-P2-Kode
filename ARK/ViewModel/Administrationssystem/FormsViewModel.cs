using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsViewModel : PageContainerViewModelBase, IContentViewModelBase, IFilterContentViewModel
    {
        private List<DamageForm> _damageFormsNonFiltered;
        private List<LongDistanceForm> _longTripFormsNonFiltered;
        private IEnumerable<DamageForm> _damageForms;
        private IEnumerable<LongDistanceForm> _longTripForms;
        private Visibility _showDamageForms;
        private Visibility _showLongTripForms;

        private DbArkContext _dbArkContext;

        public FormsViewModel()
        {
            // Instaliser lister
            _damageFormsNonFiltered = new List<DamageForm>();
            _longTripFormsNonFiltered = new List<LongDistanceForm>();

            // Opret dbcontext
            _dbArkContext = DbArkContext.GetDbContext();

            // Load data
            Task.Factory.StartNew(() =>
            {
                lock (_dbArkContext)
                {
                    // Opret forbindelser Async
                    Task<List<DamageForm>> damageforms = _dbArkContext.DamageForm.ToListAsync();
                    Task<List<LongDistanceForm>> longDistanceForms = _dbArkContext.LongTripForm.ToListAsync();

                    _damageFormsNonFiltered = damageforms.Result.ToList();
                    _longTripFormsNonFiltered = longDistanceForms.Result.ToList();
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

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return
                    GetCommand<DamageForm>(
                        e =>
                        {
                            NavigateToPage(new Func<FrameworkElement>(() => new FormsDamage()),
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
                            NavigateToPage(new Func<FrameworkElement>(() => new FormsLongTrip()),
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

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
               (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
                return;

            // Tjek filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                LongDistanceForms = FilterContent.FilterItems(LongDistanceForms, args.FilterEventArgs);
                DamageForms = FilterContent.FilterItems(DamageForms, args.FilterEventArgs);
            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                DamageForms = from damage in DamageForms
                              where damage.FilterDamageForms(args.SearchEventArgs.SearchText)
                                   select damage;

                LongDistanceForms = from form in LongDistanceForms
                                    where form.FilterLongDistanceForm(args.SearchEventArgs.SearchText)
                    select form;
            }

            ShowDamageForms = DamageForms.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            ShowLongDistanceForms = LongDistanceForms.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public FrameworkElement Filter
        {
            get { return new FormsFilter(); }
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly DbArkContext _dbArkContext;
        private IEnumerable<DamageForm> _damageForms;
        private List<DamageForm> _damageFormsNonFiltered;
        private IEnumerable<LongTripForm> _longTripForms;
        private List<LongTripForm> _longTripFormsNonFiltered;
        private int _selectedIndexDamageForms;
        private Visibility _showDamageForms;
        private Visibility _showLongTripForms;
        private int _selectedTabIndex;


        public FormsViewModel()
        {
            // Instaliser lister
            _damageFormsNonFiltered = new List<DamageForm>();
            _longTripFormsNonFiltered = new List<LongTripForm>();

            // Opret dbcontext
            _dbArkContext = DbArkContext.GetDbContext();

            // Load data
            Task.Factory.StartNew(() =>
            {
                lock (_dbArkContext)
                {
                    // Opret forbindelser Async
                    Task<List<DamageForm>> damageforms = _dbArkContext.DamageForm.ToListAsync();
                    Task<List<LongTripForm>> longDistanceForms = _dbArkContext.LongTripForm.ToListAsync();

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

            //if (DamageForms.Count() != 0)
            //{
            //    SelectedIndexDamageForms = 0;
            //}
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

        public IEnumerable<LongTripForm> LongDistanceForms
        {
            get { return _longTripForms; }
            private set
            {
                _longTripForms = value;
                Notify();
            }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { _selectedTabIndex = value; Notify(); }
        }

        #region Commands

        public int SelectedIndexDamageForms
        {
            get { return _selectedIndexDamageForms; }
            set
            {
                _selectedIndexDamageForms = value;
                Notify();
            }
        }

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return
                    GetCommand<DamageForm>(
                        e =>
                        {
                            if (e == null) return;

                            GoToDamageForm(e);
                        });
            }
        }

        public void GoToDamageForm(DamageForm df) 
        {

            NavigateToPage(() => new FormsDamage(),
                df.Description);

            // Sæt damageform
            var vm = CurrentPage.DataContext as FormsDamageViewModel;
            if (vm != null)
                vm.DamageForm = df;
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return GetCommand<LongTripForm>( e =>
                {
                    if (e == null) return;

                    GoToLongDistanceForm(e);
                });
            }
        }

        public void GoToLongDistanceForm(LongTripForm ltf)
        {

            NavigateToPage(() => new FormsLongTrip(),
                ltf.TourDescription);

            var vm = CurrentPage.DataContext as FormsLongTripViewModel;
            if (vm != null)
                vm.LongDistanceForm = ltf;
        }

        #endregion

        #region Filters

        public IFilterContainerViewModel FilterContainer
        {
            get { return Parent as IFilterContainerViewModel; }
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

        public FrameworkElement Filter
        {
            get { return new FormsFilter(); }
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
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                DamageForms = from damage in DamageForms
                    where damage.Filter(args.SearchEventArgs.SearchText)
                    select damage;

                LongDistanceForms = from form in LongDistanceForms
                    where form.Filter(args.SearchEventArgs.SearchText)
                    select form;
            }

            ShowDamageForms = DamageForms.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            ShowLongDistanceForms = LongDistanceForms.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        #endregion

        #region IContentViewModelBase

        private IViewModelBase _parent;

        public IViewModelBase Parent
        {
            get { return _parent; }
            set
            {
                if (value != null)
                {
                    // Attach
                    _parent = value;

                    if (ParentAttached != null)
                        ParentAttached(this, new EventArgs());
                }
                else
                {
                    // Detach
                    if (ParentDetached != null)
                        ParentDetached(this, new EventArgs());

                    _parent = null;
                }
            }
        }

        public event EventHandler ParentAttached;
        public event EventHandler ParentDetached;

        #endregion

        public ICommand GotFocus
        {
            get { return GetCommand<FrameworkElement>(element => ContentViewModelBase.GetKeyboard(this).GotFocus.Execute(element)); }
        }
    }
}
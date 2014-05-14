using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.View.Administrationssystem.Pages;
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
        private int _selectedIndexLongDistanceForms;
        private int _selectedTabIndex;

        public FormsViewModel()
        {
            // Opret dbcontext
            _dbArkContext = DbArkContext.GetDbContext();

            // Load data
            ParentAttached += (sender, e) =>
            {
                using (var db = new DbArkContext())
                {
                    _damageFormsNonFiltered = db.DamageForm
                        .Include(form => form.RegisteringMember)
                        .Include(form => form.Boat)
                        .OrderBy(form => form.Closed)
                        .ToList();

                    _longTripFormsNonFiltered = db.LongTripForm
                        .Include(form => form.Boat)
                        .Include(form => form.Members)
                        .Include(form => form.ResponsibleMember)
                        .ToList();
                }

                // Reset filter
                ResetFilter();

                // Set selected tab index to 0 - damage
                if (DamageForms.Any())
                {
                    SelectedTabIndex = 0;
                    SelectedIndexDamageForms = 0;
                    GoToDamageForm(DamageForms.First());
                }
            };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public IEnumerable<DamageForm> DamageForms
        {
            get { return _damageForms; }
            set
            {
                _damageForms = value;
                Notify();
            }
        }

        public IEnumerable<LongTripForm> LongDistanceForms
        {
            get { return _longTripForms; }
            set
            {
                _longTripForms = value;
                Notify();
            }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                Notify();

                switch (_selectedTabIndex)
                {
                    case 0:
                        DamageForm = true;
                        if (DamageForms.Any())
                        {
                            DamageForm df = DamageForms.First();
                            NavigateToPage(() => new FormsDamage(), df.Description); // Hvorfor er det nødvendigt at Description bliver sendt med? -Martin

                            // Sæt damageform
                            var vm = CurrentPage.DataContext as FormsDamageViewModel;
                            if (vm != null)
                                vm.DamageForm = df;

                            //SelectedIndexDamageForms = 0;
                        }
                        var templdf = LongDistanceForms;
                        LongDistanceForms = null;
                        LongDistanceForms = templdf;
                        break;
                    case 1:
                        DamageForm = false;

                        if (LongDistanceForms.Any())
                        {
                            LongTripForm ldf = LongDistanceForms.First();
                            NavigateToPage(() => new FormsLongTrip(), ldf.TourDescription); // Hvorfor er det nødvendigt at Description bliver sendt med? -Martin

                            // Sæt damageform
                            var vm = CurrentPage.DataContext as FormsLongTripViewModel;
                            if (vm != null)
                                vm.LongDistanceForm = ldf;

                            //SelectedIndexLongDistanceForms = 0;
                        }

                        //GoToLongDistanceForm(LongDistanceForms.First());
                        var tempdmf = DamageForms;
                        DamageForms = null;
                        DamageForms = tempdmf;
                        break;
                }
            }
        }

        public int SelectedIndexLongDistanceForms
        {
            get { return _selectedIndexLongDistanceForms; }
            set
            {
                _selectedIndexLongDistanceForms = value;
                Notify();
            }
        }

        public int SelectedIndexDamageForms
        {
            get { return _selectedIndexDamageForms; }
            set
            {
                _selectedIndexDamageForms = value;
                Notify();
            }
        }

        private bool DamageForm
        {
            get { return _damageForm; }
            set
            {
                _damageForm = value;
                UpdateFilter();
            }
        }

        public ICommand GotFocus
        {
            get
            {
                return
                    GetCommand(
                        element => ContentViewModelBase.GetKeyboard(this).GotFocus.Execute((FrameworkElement)element));
            }
        }

        private void UpdateFilter()
        {
            var vm = Parent as AdminSystemViewModel;
            if (vm != null)
                vm.UpdateFilter();
        }

        #region Commands



        public ICommand SelectDamageFormCommand
        {
            get
            {
                return
                    GetCommand(
                        e =>
                        {
                            if (e == null) return;

                            GoToDamageForm((DamageForm)e);
                        });
            }
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return GetCommand(e =>
                {
                    if (e == null) return;

                    GoToLongDistanceForm((LongTripForm)e);
                });
            }
        }

        public void OpenDamageForm(DamageForm damageForm)
        {
            GoToDamageForm(DamageForms.First(form => form.Id == damageForm.Id));
        }

        public void OpenLongDistanceForm(LongTripForm longTrip)
        {
            GoToLongDistanceForm(LongDistanceForms.First(form => form.Id == longTrip.Id));
        }

        private void GoToDamageForm(DamageForm df)
        {
            // Set tab index
            SelectedTabIndex = 0;

            NavigateToPage(() => new FormsDamage(),
                df.Description);

            // Sæt damageform
            var vm = CurrentPage.DataContext as FormsDamageViewModel;
            if (vm != null)
                vm.DamageForm = df;
        }

        private void GoToLongDistanceForm(LongTripForm ltf)
        {
            // Set tab index
            SelectedTabIndex = 1;
            
            NavigateToPage(() => new FormsLongTrip(),
                ltf.TourDescription);

            var vm = CurrentPage.DataContext as FormsLongTripViewModel;
            if (vm != null)
                vm.LongDistanceForm = ltf;
        }

        #endregion

        #region Filters

        private FrameworkElement _filterDamage;
        private FrameworkElement _filterLongDistance;

        public IFilterContainerViewModel FilterContainer
        {
            get { return Parent as IFilterContainerViewModel; }
        }

        public FrameworkElement Filter
        {
            get
            {
                return DamageForm
                    ? _filterDamage ?? (_filterDamage = new FormsFilterDamage())
                    : _filterLongDistance ?? (_filterLongDistance = new FormsFilter());
            }
        }

        private void ResetFilter()
        {
            DamageForms = _damageFormsNonFiltered.AsReadOnly()
                .OrderBy(d => d.Closed).ThenBy(d => d.Date);


            LongDistanceForms = _longTripFormsNonFiltered.AsReadOnly()
                .OrderBy(x => x.Status).ThenBy(x => x.FormCreated);
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
        }

        #endregion

        #region IContentViewModelBase

        private bool _damageForm;
        private bool _longTripForm;
        private IViewModelBase _parent;
        private int _tabSelectedIndex;

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
    }
}
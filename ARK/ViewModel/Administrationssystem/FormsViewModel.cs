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
        #region Fields

        private readonly DbArkContext _dbArkContext;

        private bool _damageForm;

        private IEnumerable<DamageForm> _damageForms;

        private List<DamageForm> _damageFormsNonFiltered;

        private FrameworkElement _filterDamage;

        private FrameworkElement _filterLongDistance;

        private bool _longTripForm;

        private IEnumerable<LongTripForm> _longTripForms;

        private List<LongTripForm> _longTripFormsNonFiltered;

        private IViewModelBase _parent;

        private int _selectedIndexDamageForms;

        private int _selectedIndexLongDistanceForms;

        private int _selectedTabIndex;

        private int _tabSelectedIndex;

        #endregion

        #region Constructors and Destructors

        public FormsViewModel()
        {
            // Opret dbcontext
            this._dbArkContext = DbArkContext.GetDbContext();

            // Load data
            this.ParentAttached += (sender, e) =>
                {
                    using (var db = new DbArkContext())
                    {
                        this._damageFormsNonFiltered =
                            db.DamageForm.Include(form => form.RegisteringMember)
                                .Include(form => form.Boat)
                                .OrderBy(form => form.Closed)
                                .ToList();

                        this._longTripFormsNonFiltered =
                            db.LongTripForm.Include(form => form.Boat)
                                .Include(form => form.Members)
                                .Include(form => form.ResponsibleMember)
                                .ToList();
                    }

                    // Reset filter
                    this.ResetFilter();

                    // Set selected tab index to 0 - damage
                    if (this.DamageForms.Any())
                    {
                        this.SelectedTabIndex = 0;
                        this.SelectedIndexDamageForms = 0;
                        this.GoToDamageForm(this.DamageForms.First());
                    }
                };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => this.UpdateFilter(eventArgs);
        }

        #endregion

        #region Public Events

        public event EventHandler ParentAttached;

        public event EventHandler ParentDetached;

        #endregion

        #region Public Properties

        public IEnumerable<DamageForm> DamageForms
        {
            get
            {
                return this._damageForms;
            }

            set
            {
                this._damageForms = value;
                this.Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return this.DamageForm
                           ? this._filterDamage ?? (this._filterDamage = new FormsFilterDamage())
                           : this._filterLongDistance ?? (this._filterLongDistance = new FormsFilter());
            }
        }

        public IFilterContainerViewModel FilterContainer
        {
            get
            {
                return this.Parent as IFilterContainerViewModel;
            }
        }

        public ICommand GotFocus
        {
            get
            {
                return
                    this.GetCommand(
                        element => ContentViewModelBase.GetKeyboard(this).GotFocus.Execute((FrameworkElement)element));
            }
        }

        public IEnumerable<LongTripForm> LongDistanceForms
        {
            get
            {
                return this._longTripForms;
            }

            set
            {
                this._longTripForms = value;
                this.Notify();
            }
        }

        public IViewModelBase Parent
        {
            get
            {
                return this._parent;
            }

            set
            {
                if (value != null)
                {
                    // Attach
                    this._parent = value;

                    if (this.ParentAttached != null)
                    {
                        this.ParentAttached(this, new EventArgs());
                    }
                }
                else
                {
                    // Detach
                    if (this.ParentDetached != null)
                    {
                        this.ParentDetached(this, new EventArgs());
                    }

                    this._parent = null;
                }
            }
        }

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            if (e == null)
                            {
                                return;
                            }

                            this.GoToDamageForm((DamageForm)e);
                        });
            }
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            if (e == null)
                            {
                                return;
                            }

                            this.GoToLongDistanceForm((LongTripForm)e);
                        });
            }
        }

        public int SelectedIndexDamageForms
        {
            get
            {
                return this._selectedIndexDamageForms;
            }

            set
            {
                this._selectedIndexDamageForms = value;
                this.Notify();
            }
        }

        public int SelectedIndexLongDistanceForms
        {
            get
            {
                return this._selectedIndexLongDistanceForms;
            }

            set
            {
                this._selectedIndexLongDistanceForms = value;
                this.Notify();
            }
        }

        public int SelectedTabIndex
        {
            get
            {
                return this._selectedTabIndex;
            }

            set
            {
                this._selectedTabIndex = value;
                this.Notify();

                switch (this._selectedTabIndex)
                {
                    case 0:
                        this.DamageForm = true;
                        if (this.DamageForms.Any())
                        {
                            DamageForm df = this.DamageForms.First();
                            this.NavigateToPage(() => new FormsDamage(), df.Description);
                                
                                // Hvorfor er det nødvendigt at Description bliver sendt med? -Martin

                            // Sæt damageform
                            var vm = this.CurrentPage.DataContext as FormsDamageViewModel;
                            if (vm != null)
                            {
                                vm.DamageForm = df;
                            }

                            // SelectedIndexDamageForms = 0;
                        }

                        var templdf = this.LongDistanceForms;
                        this.LongDistanceForms = null;
                        this.LongDistanceForms = templdf;
                        break;
                    case 1:
                        this.DamageForm = false;

                        if (this.LongDistanceForms.Any())
                        {
                            LongTripForm ldf = this.LongDistanceForms.First();
                            this.NavigateToPage(() => new FormsLongTrip(), ldf.TourDescription);
                                
                                // Hvorfor er det nødvendigt at Description bliver sendt med? -Martin

                            // Sæt damageform
                            var vm = this.CurrentPage.DataContext as FormsLongTripViewModel;
                            if (vm != null)
                            {
                                vm.LongDistanceForm = ldf;
                            }

                            // SelectedIndexLongDistanceForms = 0;
                        }

                        // GoToLongDistanceForm(LongDistanceForms.First());
                        var tempdmf = this.DamageForms;
                        this.DamageForms = null;
                        this.DamageForms = tempdmf;
                        break;
                }
            }
        }

        #endregion

        #region Properties

        private bool DamageForm
        {
            get
            {
                return this._damageForm;
            }

            set
            {
                this._damageForm = value;
                this.UpdateFilter();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void OpenDamageForm(DamageForm damageForm)
        {
            this.GoToDamageForm(this.DamageForms.First(form => form.Id == damageForm.Id));
        }

        public void OpenLongDistanceForm(LongTripForm longTrip)
        {
            this.GoToLongDistanceForm(this.LongDistanceForms.First(form => form.Id == longTrip.Id));
        }

        #endregion

        #region Methods

        private void GoToDamageForm(DamageForm df)
        {
            // Set tab index
            this.SelectedTabIndex = 0;

            this.NavigateToPage(() => new FormsDamage(), df.Description);

            // Sæt damageform
            var vm = this.CurrentPage.DataContext as FormsDamageViewModel;
            if (vm != null)
            {
                vm.DamageForm = df;
            }
        }

        private void GoToLongDistanceForm(LongTripForm ltf)
        {
            // Set tab index
            this.SelectedTabIndex = 1;

            this.NavigateToPage(() => new FormsLongTrip(), ltf.TourDescription);

            var vm = this.CurrentPage.DataContext as FormsLongTripViewModel;
            if (vm != null)
            {
                vm.LongDistanceForm = ltf;
            }
        }

        private void ResetFilter()
        {
            this.DamageForms = this._damageFormsNonFiltered.AsReadOnly().OrderBy(d => d.Closed).ThenBy(d => d.Date);

            this.LongDistanceForms =
                this._longTripFormsNonFiltered.AsReadOnly().OrderBy(x => x.Status).ThenBy(x => x.FormCreated);
        }

        private void UpdateFilter()
        {
            var vm = this.Parent as AdminSystemViewModel;
            if (vm != null)
            {
                vm.UpdateFilter();
            }
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Nulstil filter
            this.ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Tjek filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.LongDistanceForms = FilterContent.FilterItems(this.LongDistanceForms, args.FilterEventArgs);
                this.DamageForms = FilterContent.FilterItems(this.DamageForms, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.DamageForms = from damage in this.DamageForms
                                   where damage.Filter(args.SearchEventArgs.SearchText)
                                   select damage;

                this.LongDistanceForms = from form in this.LongDistanceForms
                                         where form.Filter(args.SearchEventArgs.SearchText)
                                         select form;
            }
        }

        #endregion
    }
}
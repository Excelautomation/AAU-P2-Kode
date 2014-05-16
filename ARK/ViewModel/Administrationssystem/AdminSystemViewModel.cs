using System;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.View.Administrationssystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class AdminSystemViewModel : KeyboardContainerViewModelBase, IFilterContainerViewModel
    {
        #region Fields

        private Admin _currentLoggedInUser;

        private bool _enableFilters;

        private bool _enableSearch;

        private FrameworkElement _filter;

        private Baede _pageBoats;

        private Blanketter _pageForms;

        private Oversigt _pageOverview;

        private Trips _pageTrips;

        private string _searchText;

        #endregion

        #region Constructors and Destructors

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            this.MenuOverview.Execute(null);

            TimeCounter.StopTime();
        }

        #endregion

        #region Public Events

        public event EventHandler<FilterEventArgs> FilterTextChanged;

        public event EventHandler<SearchEventArgs> SearchTextChanged;

        #endregion

        #region Public Properties

        public int ContentRow
        {
            get
            {
                return this.EnableSearch && this.EnableFilters ? 2 : 0;
            }
        }

        public int ContentRowSpan
        {
            get
            {
                return this.EnableSearch && this.EnableFilters ? 1 : 3;
            }
        }

        public Admin CurrentLoggedInUser
        {
            get
            {
                return this._currentLoggedInUser;
            }

            set
            {
                this._currentLoggedInUser = value;
                this.Notify();
            }
        }

        public bool EnableFilters
        {
            get
            {
                return this._enableFilters;
            }

            set
            {
                this._enableFilters = value;
                this.Notify();
                this.NotifyFilter();
            }
        }

        public bool EnableSearch
        {
            get
            {
                return this._enableSearch;
            }

            set
            {
                this._enableSearch = value;
                this.Notify();
                this.NotifyFilter();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return this._filter;
            }

            set
            {
                IFilterViewModel filterViewModel;

                if (this._filter != null)
                {
                    // Unbind event
                    filterViewModel = this._filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null)
                    {
                        filterViewModel.FilterChanged -= this.filter_FilterChanged;
                    }
                }

                this._filter = value;

                // Tjek at filter ikke er null
                if (this._filter != null)
                {
                    // Bind event
                    filterViewModel = this._filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null)
                    {
                        filterViewModel.FilterChanged += this.filter_FilterChanged;
                    }
                }

                this.Notify();
            }
        }

        public Visibility FilterBarVisibility
        {
            get
            {
                return this.EnableSearch && this.EnableFilters ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility FiltersVisibility
        {
            get
            {
                return this.EnableFilters ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand Logout
        {
            get
            {
                return this.GetCommand(e => ((Window)e).Close());
            }
        }

        public ICommand MenuBoats
        {
            get
            {
                return this.GetNavigateCommand(() => this.PageBoats, "Boats");
            }
        }

        public ICommand MenuConfigurations
        {
            get
            {
                return this.GetNavigateCommand(() => this.PageConfigurations, "Configurations");
            }
        }

        public ICommand MenuForms
        {
            get
            {
                return this.GetNavigateCommand(() => this.PageForms, "Forms");
            }
        }

        public ICommand MenuOverview
        {
            get
            {
                return this.GetNavigateCommand(() => this.PageOverview, "Overview");
            }
        }

        public ICommand MenuTrips
        {
            get
            {
                return this.GetNavigateCommand(() => this.PageTrips, "Trips");
            }
        }

        public ICommand SearchChangedCommand
        {
            get
            {
                return this.GetCommand(
                    s =>
                        {
                            if (this.SearchTextChanged != null)
                            {
                                this.SearchTextChanged(this, new SearchEventArgs((string)s));
                            }
                        });
            }
        }

        public string SearchText
        {
            get
            {
                return this._searchText;
            }

            set
            {
                this._searchText = value;
                this.Notify();
            }
        }

        public Visibility SearchVisibility
        {
            get
            {
                return this.EnableSearch ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region Properties

        private Baede PageBoats
        {
            get
            {
                return this._pageBoats ?? (this._pageBoats = new Baede());
            }
        }

        private Indstillinger PageConfigurations
        {
            get
            {
                return new Indstillinger();
            }
        }

        private Blanketter PageForms
        {
            get
            {
                return this._pageForms ?? (this._pageForms = new Blanketter());
            }
        }

        private Oversigt PageOverview
        {
            get
            {
                return this._pageOverview ?? (this._pageOverview = new Oversigt());
            }
        }

        private Trips PageTrips
        {
            get
            {
                return this._pageTrips ?? (this._pageTrips = new Trips());
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            this.EnableSearch = false;
            this.EnableFilters = false;

            base.NavigateToPage(page, pageTitle);

            this.SearchText = string.Empty;

            // Set filter
            this.UpdateFilter();
        }

        public void UpdateFilter()
        {
            var viewModelbase = this.CurrentPage.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                this.Filter = viewModelbase.Filter;
            }
            else
            {
                this.Filter = null;
            }
        }

        #endregion

        #region Methods

        private void NotifyFilter()
        {
            this.NotifyCustom("SearchVisibility");
            this.NotifyCustom("FiltersVisibility");
            this.NotifyCustom("FilterBarVisibility");
            this.NotifyCustom("ContentRow");
            this.NotifyCustom("ContentRowSpan");
        }

        private void filter_FilterChanged(object sender, FilterEventArgs e)
        {
            if (this.FilterTextChanged != null)
            {
                this.FilterTextChanged(sender, e);
            }
        }

        #endregion
    }
}
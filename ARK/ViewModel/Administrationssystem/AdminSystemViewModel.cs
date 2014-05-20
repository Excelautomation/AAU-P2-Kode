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
        private Admin _currentLoggedInUser;

        private bool _enableFilters;

        private bool _enableSearch;

        private FrameworkElement _filter;

        private Baede _pageBoats;

        private Blanketter _pageForms;

        private Oversigt _pageOverview;

        private Trips _pageTrips;

        private string _searchText;

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            MenuOverview.Execute(null);

            TimeCounter.StopTime();
        }

        public int ContentRow
        {
            get
            {
                return EnableSearch && EnableFilters ? 2 : 0;
            }
        }

        public int ContentRowSpan
        {
            get
            {
                return EnableSearch && EnableFilters ? 1 : 3;
            }
        }

        public Admin CurrentLoggedInUser
        {
            get
            {
                return _currentLoggedInUser;
            }

            set
            {
                _currentLoggedInUser = value;
                Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                IFilterViewModel filterViewModel;

                if (_filter != null)
                {
                    // Unbind event
                    filterViewModel = _filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null)
                    {
                        filterViewModel.FilterChanged -= filter_FilterChanged;
                    }
                }

                _filter = value;

                // Tjek at filter ikke er null
                if (_filter != null)
                {
                    // Bind event
                    filterViewModel = _filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null)
                    {
                        filterViewModel.FilterChanged += filter_FilterChanged;
                    }
                }

                Notify();
            }
        }

        public Visibility FilterBarVisibility
        {
            get
            {
                return EnableSearch && EnableFilters ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility FiltersVisibility
        {
            get
            {
                return EnableFilters ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand Logout
        {
            get
            {
                return GetCommand(e => ((Window)e).Close());
            }
        }

        public ICommand MenuBoats
        {
            get
            {
                return GetNavigateCommand(() => PageBoats, "Boats");
            }
        }

        public ICommand MenuConfigurations
        {
            get
            {
                return GetNavigateCommand(() => PageConfigurations, "Configurations");
            }
        }

        public ICommand MenuForms
        {
            get
            {
                return GetNavigateCommand(() => PageForms, "Forms");
            }
        }

        public ICommand MenuOverview
        {
            get
            {
                return GetNavigateCommand(() => PageOverview, "Overview");
            }
        }

        public ICommand MenuTrips
        {
            get
            {
                return GetNavigateCommand(() => PageTrips, "Trips");
            }
        }

        public ICommand SearchChangedCommand
        {
            get
            {
                return GetCommand(
                    s =>
                        {
                            if (SearchTextChanged != null)
                            {
                                SearchTextChanged(this, new SearchEventArgs((string)s));
                            }
                        });
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                _searchText = value;
                Notify();
            }
        }

        public Visibility SearchVisibility
        {
            get
            {
                return EnableSearch ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Baede PageBoats
        {
            get
            {
                return _pageBoats ?? (_pageBoats = new Baede());
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
                return _pageForms ?? (_pageForms = new Blanketter());
            }
        }

        private Oversigt PageOverview
        {
            get
            {
                return _pageOverview ?? (_pageOverview = new Oversigt());
            }
        }

        private Trips PageTrips
        {
            get
            {
                return _pageTrips ?? (_pageTrips = new Trips());
            }
        }

        public event EventHandler<FilterEventArgs> FilterTextChanged;

        public event EventHandler<SearchEventArgs> SearchTextChanged;

        public bool EnableFilters
        {
            get
            {
                return _enableFilters;
            }

            set
            {
                _enableFilters = value;
                Notify();
                NotifyFilter();
            }
        }

        public bool EnableSearch
        {
            get
            {
                return _enableSearch;
            }

            set
            {
                _enableSearch = value;
                Notify();
                NotifyFilter();
            }
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            EnableSearch = false;
            EnableFilters = false;

            base.NavigateToPage(page, pageTitle);

            SearchText = string.Empty;

            // Set filter
            UpdateFilter();
        }

        public void UpdateFilter()
        {
            var viewModelbase = CurrentPage.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                Filter = viewModelbase.Filter;
            }
            else
            {
                Filter = null;
            }
        }

        private void NotifyFilter()
        {
            NotifyCustom("SearchVisibility");
            NotifyCustom("FiltersVisibility");
            NotifyCustom("FilterBarVisibility");
            NotifyCustom("ContentRow");
            NotifyCustom("ContentRowSpan");
        }

        private void filter_FilterChanged(object sender, FilterEventArgs e)
        {
            if (FilterTextChanged != null)
            {
                FilterTextChanged(sender, e);
            }
        }
    }
}
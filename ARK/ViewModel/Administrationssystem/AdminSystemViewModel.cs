using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class AdminSystemViewModel : PageContainerViewModelBase, IFilterContainerViewModel
    {
        private readonly ObservableCollection<FrameworkElement> _filters = new ObservableCollection<FrameworkElement>();
        private bool _enableFilters;
        private bool _enableSearch;
        private Baede _pagebaede;
        private Blanketter _pageforms;
        private Oversigt _pageoversigt;
        private Indstillinger _pagesettings;

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            MenuOverview.Execute(null);

            TimeCounter.StopTime("AdministrationssystemViewModel load");
        }

        public override void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle)
        {
            EnableSearch = false;
            EnableFilters = false;
            Filters.Clear();

            base.NavigateToPage(page, pageTitle);
        }

        #region Pages

        public ICommand MenuOverview
        {
            get { return GetNavigateCommand(new Lazy<FrameworkElement>(() => PageOverview), "Overview"); }
        }

        public ICommand MenuForms
        {
            get { return GetNavigateCommand(new Lazy<FrameworkElement>(() => PageForms), "Forms"); }
        }

        public ICommand MenuBoats
        {
            get { return GetNavigateCommand(new Lazy<FrameworkElement>(() => PageBoats), "Boats"); }
        }

        public ICommand MenuConfigurations
        {
            get { return GetNavigateCommand(new Lazy<FrameworkElement>(() => PageConfigurations), "Configurations"); }
        }

        private Oversigt PageOverview
        {
            get { return _pageoversigt ?? (_pageoversigt = new Oversigt()); }
        }

        private Blanketter PageForms
        {
            get { return _pageforms ?? (_pageforms = new Blanketter()); }
        }

        private Baede PageBoats
        {
            get { return _pagebaede ?? (_pagebaede = new Baede()); }
        }

        private Indstillinger PageConfigurations
        {
            get { return _pagesettings ?? (_pagesettings = new Indstillinger()); }
        }

        #endregion private

        #region Filter

        public ICommand SearchChangedCommand
        {
            get
            {
                return
                    GetCommand<string>(
                        s => { if (SearchTextChanged != null) SearchTextChanged(this, new SearchEventArgs(s)); });
            }
        }

        public Visibility SearchVisibility
        {
            get { return EnableSearch ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility FiltersVisibility
        {
            get { return EnableFilters ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility FilterBarVisibility
        {
            get { return EnableSearch && EnableFilters ? Visibility.Visible : Visibility.Collapsed; }
        }

        public int ContentRow
        {
            get { return EnableSearch && EnableFilters ? 2 : 0; }
        }

        public int ContentRowSpan
        {
            get { return EnableSearch && EnableFilters ? 1 : 3; }
        }

        private void NotifyFilter()
        {
            NotifyCustom("SearchVisibility");
            NotifyCustom("FiltersVisibility");
            NotifyCustom("FilterBarVisibility");
            NotifyCustom("ContentRow");
            NotifyCustom("ContentRowSpan");
        }

        public event EventHandler<SearchEventArgs> SearchTextChanged;

        public bool EnableSearch
        {
            get { return _enableSearch; }
            set
            {
                _enableSearch = value;
                Notify();
                NotifyFilter();
            }
        }

        public bool EnableFilters
        {
            get { return _enableFilters; }
            set
            {
                _enableFilters = value;
                Notify();
                NotifyFilter();
            }
        }

        public ObservableCollection<FrameworkElement> Filters
        {
            get { return _filters; }
        }

        #endregion
    }
}
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.ViewModel.Base;
using ARK.Model;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class AdminSystemViewModel : KeyboardContainerViewModelBase, IFilterContainerViewModel
    {
        private bool _enableFilters;
        private bool _enableSearch;
        private string _searchText;

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            MenuOverview.Execute(null);

            // Setup keyboard listener
            KeyboardTextChanged += (sender, e) =>
            {
                if (CurrentSelectedTextBox == null) return;

                var textBox = CurrentSelectedTextBox as TextBox;
                if (textBox != null)
                    textBox.Text = KeyboardText;
            };

            TimeCounter.StopTime();
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            FrameworkElement element = page();

            EnableSearch = false;
            EnableFilters = false;
            SearchText = "";

            // Sæt filter
            var viewModelbase = element.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                Filter = viewModelbase.Filter;
            }
            else
                Filter = null;

            base.NavigateToPage(() => element, pageTitle);
        }

        private Admin currentLoggetInUser;

        public Admin CurrentLoggedInUser
        {
            get { return currentLoggetInUser; }
            set { currentLoggetInUser = value; Notify(); }
        }
        
        #region Pages

        public ICommand MenuOverview
        {
            get { return GetNavigateCommand(() => PageOverview, "Overview"); }
        }

        public ICommand MenuForms
        {
            get { return GetNavigateCommand(() => PageForms, "Forms"); }
        }

        public ICommand MenuBoats
        {
            get { return GetNavigateCommand(() => PageBoats, "Boats"); }
        }

        public ICommand MenuConfigurations
        {
            get { return GetNavigateCommand(() => PageConfigurations, "Configurations"); }
        }

        private Oversigt PageOverview
        {
            get { return new Oversigt(); }
        }

        private Blanketter PageForms
        {
            get { return new Blanketter(); }
        }

        private Baede PageBoats
        {
            get { return new Baede(); }
        }

        private Indstillinger PageConfigurations
        {
            get { return new Indstillinger(); }
        }

        #endregion private

        #region Filter

        public FrameworkElement Filter
        {
            get { return _filter; }
            set
            {
                IFilterViewModel filterViewModel;

                if (_filter != null)
                {
                    // Unbind event
                    filterViewModel = _filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null) filterViewModel.FilterChanged -= filter_FilterChanged;
                }

                _filter = value;

                // Tjek at filter ikke er null
                if (_filter != null)
                {
                    // Bind event
                    filterViewModel = _filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null) filterViewModel.FilterChanged += filter_FilterChanged;
                }

                Notify();
            }
        }

        private void filter_FilterChanged(object sender, FilterEventArgs e)
        {
            if (FilterTextChanged != null)
                FilterTextChanged(sender, e);
        }

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; Notify(); }
        }

        public ICommand SearchChangedCommand
        {
            get
            {
                return
                    GetCommand<string>(
                        s =>
                        {
                            if (SearchTextChanged != null) 
                                SearchTextChanged(this, new SearchEventArgs(s));
                        });
            }
        }

        private FrameworkElement CurrentSelectedTextBox { get; set; }

        public ICommand GotFocus
        {
            get
            {
                return GetCommand<FrameworkElement>(element =>
                {
                    CurrentSelectedTextBox = element;
                });
            }
        }

        public override ICommand KeyboardToggle
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // Check if a textbox has been selected
                    if (CurrentSelectedTextBox == null)
                        return;
                    
                    // Change keyboard state
                    base.KeyboardToggled = !base.KeyboardToggled;

                    // Update keyboard text
                    var textbox = CurrentSelectedTextBox as TextBox;
                    if (textbox != null)
                        KeyboardText = textbox.Text;
                });
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
        public event EventHandler<FilterEventArgs> FilterTextChanged;
        private FrameworkElement _filter;

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

        #endregion
    }
}
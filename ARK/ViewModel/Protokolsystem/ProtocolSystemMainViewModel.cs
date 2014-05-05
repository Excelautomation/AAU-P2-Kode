﻿using System;
using System.Windows;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : KeyboardContainerViewModelBase,
        IFilterContainerViewModel, IInfoContainerViewModel
    {
        #region PrivateFields

        private ICommand _boatsOut;
        private ICommand _createDamage;
        private ICommand _createLongDistance;
        private FrameworkElement _currentInfo;
        private bool _enableFilters;
        private ICommand _endTrip;
        private FrameworkElement _filter;
        private string _headlineText;
        private ICommand _infoScreen;
        private ICommand _memberInformation;
        private ICommand _startTrip;
        private ICommand _statisticsDistance;
        private EndTrip _endTripPage;

        #endregion

        public ProtocolSystemMainViewModel()
        {
            TimeCounter.StartTimer();

            StatisticsDistance.Execute(null);

            KeyboardTextChanged +=
                (sender, args) =>
                {
                    if (SearchTextChanged != null)
                        SearchTextChanged(sender, new SearchEventArgs(KeyboardText));
                };

            TimeCounter.StopTime();
        }

        #region Pages

        private BeginTripBoats _beginTripBoatsPage;

        public string HeadlineText
        {
            get { return _headlineText; }
            set
            {
                _headlineText = value;
                Notify();
            }
        }

        public ICommand InfoScreen // overview screen..
        {
            get { return _infoScreen ?? (_infoScreen = GetNavigateCommand(() => new MainPage(), "InfoScreen")); }
        }

        private BeginTripBoats BeginTripBoatsPage
        {
            get { return _beginTripBoatsPage ?? (_beginTripBoatsPage = new BeginTripBoats()); }
        }

        private EndTrip EndTripPage
        {
            get { return _endTripPage ?? (_endTripPage = new EndTrip()); }
        }

        public ICommand StartTrip
        {
            get
            {
                return _startTrip ??
                       (_startTrip =
                           GetNavigateCommand(() => BeginTripBoatsPage, "START ROTUR"));
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return _endTrip ??
                       (_endTrip = GetNavigateCommand(() => EndTripPage, "AFSLUT ROTUR"));
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return _boatsOut ??
                       (_boatsOut =
                           GetNavigateCommand(() => new BoatsOut(), "BÅDE PÅ VANDET"));
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return _statisticsDistance ??
                       (_statisticsDistance =
                           GetNavigateCommand(() => new DistanceStatistics(),
                               "KILOMETERSTATISTIK"));
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return _memberInformation ??
                       (_memberInformation =
                           GetNavigateCommand(() => new MembersInformation(),
                               "MEDLEMSINFORMATION"));
            }
        }

        public ICommand CreateDamage
        {
            get
            {
                return _createDamage ??
                       (_createDamage =
                           GetNavigateCommand(() => new CreateInjury(),
                               "SKADE-BLANKET"));
            }
        }

        public ICommand CreateLongDistance
        {
            get
            {
                return _createLongDistance ??
                       (_createLongDistance =
                           GetNavigateCommand(() => new CreateLongDistance(),
                               "LANGTUR-BLANKET"));
            }
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            FrameworkElement element = page();

            // Deactivate filter
            EnableSearch = false;
            EnableFilters = false;

            // Set filter
            var viewModelbase = element.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                Filter = viewModelbase.Filter;
            }
            else
                Filter = null;

            // Remove information
            CurrentInfo = null;

            base.NavigateToPage(() => element, pageTitle);
        }

        #endregion

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

                if (_filter != null)
                {
                    // Bind event
                    filterViewModel = _filter.DataContext as IFilterViewModel;
                    if (filterViewModel != null) filterViewModel.FilterChanged += filter_FilterChanged;
                }

                Notify();
            }
        }

        public event EventHandler<SearchEventArgs> SearchTextChanged;
        public event EventHandler<FilterEventArgs> FilterTextChanged;

        public bool EnableSearch
        {
            get { return KeyboardToggled; }
            set
            {
                KeyboardToggled = value;
                Notify();

                // If Keyboard is active
                if (value)
                {
                    EnableFilters = false;
                }
            }
        }

        public bool EnableFilters
        {
            get { return _enableFilters; }
            set
            {
                _enableFilters = value;
                Notify();

                if (value)
                {
                    EnableSearch = false;
                }
            }
        }

        private void filter_FilterChanged(object sender, FilterEventArgs e)
        {
            if (FilterTextChanged != null)
                FilterTextChanged(sender, e);
        }

        #endregion

        #region Info

        public FrameworkElement CurrentInfo
        {
            get { return _currentInfo; }
            private set
            {
                _currentInfo = value;
                Notify();
            }
        }

        public void ChangeInfo<T>(FrameworkElement infopage, T info)
        {
            // Check page
            if (infopage == null) throw new ArgumentNullException("infopage");

            // Set the correct infopage
            CurrentInfo = infopage;

            // Check ViewModel
            var viewModel = infopage.DataContext as ContentViewModelBase;

            // Set parent
            if (viewModel == null) return;

            viewModel.Parent = this;
        }

        #endregion
    }
}
﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : KeyboardContainerViewModelBase, 
                                               IFilterContainerViewModel, 
                                               IInfoContainerViewModel
    {
        private readonly DbArkContext db = DbArkContext.GetDbContext();

        private BeginTripBoats _beginTripBoatsPage;

        private BoatsOut _boatsOutPage;

        private FrameworkElement _currentInfo;

        private double _dailyKilometers;

        private FrameworkElement _dialogElement;

        private bool _dialogShow;

        private DistanceStatistics _distanceStatisticsPage;

        private bool _enableFilters;

        private EndTrip _endTripPage;

        private FrameworkElement _filter;

        private string _headlineText;

        private MembersInformation _membersInformationPage;

        private int _numBoatsOut;

        private ViewDamageForm _viewDamageFormPage;

        private ViewLongTripForm _viewLongTripFormPage;

        public ProtocolSystemMainViewModel()
        {
            TimeCounter.StartTimer();

            StatisticsDistance.Execute(null);

            KeyboardTextChanged += (sender, args) =>
                {
                    if (SearchTextChanged != null)
                    {
                        SearchTextChanged(sender, new SearchEventArgs(KeyboardText));
                    }
                };

            UpdateDailyKilometers();
            UpdateNumBoatsOut();

            TimeCounter.StopTime();
        }

        public ICommand AdminLogin
        {
            get
            {
                return GetCommand(() => ShowDialog(new AdminLoginConfirm()));
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return GetNavigateCommand(() => BoatsOutPage, "BÅDE PÅ VANDET");
            }
        }

        public double DailyKilometers
        {
            get
            {
                return _dailyKilometers;
            }

            set
            {
                _dailyKilometers = value;
                Notify();
            }
        }

        public FrameworkElement DialogElement
        {
            get
            {
                return _dialogElement;
            }

            set
            {
                _dialogElement = value;
                Notify();
            }
        }

        public bool DialogShow
        {
            get
            {
                return _dialogShow;
            }

            set
            {
                _dialogShow = value;
                Notify();
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return GetNavigateCommand(() => EndTripPage, "AFSLUT ROTUR");
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

        public string HeadlineText
        {
            get
            {
                return _headlineText;
            }

            set
            {
                _headlineText = value;
                Notify();
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return GetNavigateCommand(() => MembersInformationPage, "MEDLEMSINFORMATION");
            }
        }

        public int NumBoatsOut
        {
            get
            {
                return _numBoatsOut;
            }

            set
            {
                _numBoatsOut = value;
                Notify();
            }
        }

        public ICommand StartTrip
        {
            get
            {
                return GetNavigateCommand(() => BeginTripBoatsPage, "START ROTUR");
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return GetNavigateCommand(() => DistanceStatisticsPage, "KILOMETERSTATISTIK");
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return GetNavigateCommand(() => ViewDamageFormPage, "SKADEBLANKETTER");
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return GetNavigateCommand(() => ViewLongTripFormPage, "LANGTURSBLANKETTER");
            }
        }

        private BeginTripBoats BeginTripBoatsPage
        {
            get
            {
                return _beginTripBoatsPage ?? (_beginTripBoatsPage = new BeginTripBoats());
            }
        }

        private BoatsOut BoatsOutPage
        {
            get
            {
                return _boatsOutPage ?? (_boatsOutPage = new BoatsOut());
            }
        }

        private DistanceStatistics DistanceStatisticsPage
        {
            get
            {
                return _distanceStatisticsPage ?? (_distanceStatisticsPage = new DistanceStatistics());
            }
        }

        private EndTrip EndTripPage
        {
            get
            {
                return _endTripPage ?? (_endTripPage = new EndTrip());
            }
        }

        private MembersInformation MembersInformationPage
        {
            get
            {
                return _membersInformationPage ?? (_membersInformationPage = new MembersInformation());
            }
        }

        private ViewDamageForm ViewDamageFormPage
        {
            get
            {
                return _viewDamageFormPage ?? (_viewDamageFormPage = new ViewDamageForm());
            }
        }

        private ViewLongTripForm ViewLongTripFormPage
        {
            get
            {
                return _viewLongTripFormPage ?? (_viewLongTripFormPage = new ViewLongTripForm());
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

                if (value)
                {
                    EnableSearch = false;
                }
            }
        }

        public override bool KeyboardToggled
        {
            get
            {
                return base.KeyboardToggled;
            }
            set
            {
                base.KeyboardToggled = value;

                if (value)
                {
                    EnableFilters = false;
                }

                NotifyCustom("EnableSearch");
            }
        }

        protected override void KeyboardGotFocus(object element)
        {
            base.KeyboardGotFocus(element);

            KeyboardToggled = true;
        }

        public bool EnableSearch
        {
            get
            {
                return KeyboardToggled;
            }

            set
            {
                KeyboardToggled = value;
            }
        }

        public FrameworkElement CurrentInfo
        {
            get
            {
                return _currentInfo;
            }

            private set
            {
                _currentInfo = value;
                Notify();
            }
        }

        public void ChangeInfo<T>(FrameworkElement infopage, T info)
        {
            // Check page
            if (infopage == null)
            {
                throw new ArgumentNullException("infopage");
            }

            // Set the correct infopage
            CurrentInfo = infopage;

            // Check ViewModel
            var viewModel = infopage.DataContext as ContentViewModelBase;

            // Set parent
            if (viewModel == null)
            {
                return;
            }

            viewModel.Parent = this;
        }

        public void HideDialog()
        {
            DialogShow = false;
            DialogElement = null;
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            FrameworkElement element = page();

            // Remove information
            CurrentInfo = null;

            // Deactivate filter
            EnableSearch = false;
            EnableFilters = false;

            // Call base method
            base.NavigateToPage(() => element, pageTitle);

            // Set filter
            var viewModelbase = element.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                Filter = viewModelbase.Filter;
            }
            else
            {
                Filter = null;
            }
        }

        public void ShowDialog(FrameworkElement dialog)
        {
            DialogElement = dialog;

            // Check viewModel
            var viewModel = DialogElement.DataContext as IContentViewModelBase;

            // Set parent
            if (viewModel != null)
            {
                viewModel.Parent = this;
            }

            DialogShow = true;
        }

        public void UpdateDailyKilometers()
        {
            DateTime today = DateTime.Today;
            IQueryable<Trip> temp = db.Trip.Where(t => t.TripEndedTime > today);
            if (temp.Any())
            {
                DailyKilometers = temp.Sum(t => t.Distance * t.CrewCount);
            }
        }

        public void UpdateNumBoatsOut()
        {
            NumBoatsOut = db.Trip.Count(t => t.TripEndedTime == null);
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
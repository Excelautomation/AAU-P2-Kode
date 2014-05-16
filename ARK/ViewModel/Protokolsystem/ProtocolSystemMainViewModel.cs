using System;
using System.Collections.Generic;
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
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : KeyboardContainerViewModelBase, 
                                               IFilterContainerViewModel, 
                                               IInfoContainerViewModel
    {
        #region Fields

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

        #endregion

        #region Constructors and Destructors

        public ProtocolSystemMainViewModel()
        {
            TimeCounter.StartTimer();

            this.StatisticsDistance.Execute(null);

            this.KeyboardTextChanged += (sender, args) =>
                {
                    if (this.SearchTextChanged != null)
                    {
                        this.SearchTextChanged(sender, new SearchEventArgs(this.KeyboardText));
                    }
                };

            this.UpdateDailyKilometers();
            this.UpdateNumBoatsOut();

            TimeCounter.StopTime();
        }

        #endregion

        #region Public Events

        public event EventHandler<FilterEventArgs> FilterTextChanged;

        public event EventHandler<SearchEventArgs> SearchTextChanged;

        #endregion

        #region Public Properties

        public ICommand AdminLogin
        {
            get
            {
                return this.GetCommand(() => this.ShowDialog(new AdminLoginConfirm()));
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return this.GetNavigateCommand(() => this.BoatsOutPage, "BÅDE PÅ VANDET");
            }
        }

        public FrameworkElement CurrentInfo
        {
            get
            {
                return this._currentInfo;
            }

            private set
            {
                this._currentInfo = value;
                this.Notify();
            }
        }

        public double DailyKilometers
        {
            get
            {
                return this._dailyKilometers;
            }

            set
            {
                this._dailyKilometers = value;
                this.Notify();
            }
        }

        public FrameworkElement DialogElement
        {
            get
            {
                return this._dialogElement;
            }

            set
            {
                this._dialogElement = value;
                this.Notify();
            }
        }

        public bool DialogShow
        {
            get
            {
                return this._dialogShow;
            }

            set
            {
                this._dialogShow = value;
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

                if (value)
                {
                    this.EnableSearch = false;
                }
            }
        }

        public bool EnableSearch
        {
            get
            {
                return this.KeyboardToggled;
            }

            set
            {
                this.KeyboardToggled = value;
                this.Notify();

                // If Keyboard is active
                if (value)
                {
                    this.EnableFilters = false;
                }
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return this.GetNavigateCommand(() => this.EndTripPage, "AFSLUT ROTUR");
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

        public string HeadlineText
        {
            get
            {
                return this._headlineText;
            }

            set
            {
                this._headlineText = value;
                this.Notify();
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return this.GetNavigateCommand(() => this.MembersInformationPage, "MEDLEMSINFORMATION");
            }
        }

        public int NumBoatsOut
        {
            get
            {
                return this._numBoatsOut;
            }

            set
            {
                this._numBoatsOut = value;
                this.Notify();
            }
        }

        public ICommand StartTrip
        {
            get
            {
                return this.GetNavigateCommand(() => this.BeginTripBoatsPage, "START ROTUR");
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return this.GetNavigateCommand(() => this.DistanceStatisticsPage, "KILOMETERSTATISTIK");
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return this.GetNavigateCommand(() => this.ViewDamageFormPage, "SKADEBLANKETTER");
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return this.GetNavigateCommand(() => this.ViewLongTripFormPage, "LANGTURSBLANKETTER");
            }
        }

        #endregion

        #region Properties

        private BeginTripBoats BeginTripBoatsPage
        {
            get
            {
                return this._beginTripBoatsPage ?? (this._beginTripBoatsPage = new BeginTripBoats());
            }
        }

        private BoatsOut BoatsOutPage
        {
            get
            {
                return this._boatsOutPage ?? (this._boatsOutPage = new BoatsOut());
            }
        }

        private DistanceStatistics DistanceStatisticsPage
        {
            get
            {
                return this._distanceStatisticsPage ?? (this._distanceStatisticsPage = new DistanceStatistics());
            }
        }

        private EndTrip EndTripPage
        {
            get
            {
                return this._endTripPage ?? (this._endTripPage = new EndTrip());
            }
        }

        private MembersInformation MembersInformationPage
        {
            get
            {
                return this._membersInformationPage ?? (this._membersInformationPage = new MembersInformation());
            }
        }

        private ViewDamageForm ViewDamageFormPage
        {
            get
            {
                return this._viewDamageFormPage ?? (this._viewDamageFormPage = new ViewDamageForm());
            }
        }

        private ViewLongTripForm ViewLongTripFormPage
        {
            get
            {
                return this._viewLongTripFormPage ?? (this._viewLongTripFormPage = new ViewLongTripForm());
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ChangeInfo<T>(FrameworkElement infopage, T info)
        {
            // Check page
            if (infopage == null)
            {
                throw new ArgumentNullException("infopage");
            }

            // Set the correct infopage
            this.CurrentInfo = infopage;

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
            this.DialogShow = false;
            this.DialogElement = null;
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            FrameworkElement element = page();

            // Remove information
            this.CurrentInfo = null;

            // Deactivate filter
            this.EnableSearch = false;
            this.EnableFilters = false;

            // Call base method
            base.NavigateToPage(() => element, pageTitle);

            // Set filter
            var viewModelbase = element.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                this.Filter = viewModelbase.Filter;
            }
            else
            {
                this.Filter = null;
            }
        }

        public void ShowDialog(FrameworkElement dialog)
        {
            this.DialogElement = dialog;

            // Check viewModel
            var viewModel = this.DialogElement.DataContext as IContentViewModelBase;

            // Set parent
            if (viewModel != null)
            {
                viewModel.Parent = this;
            }

            this.DialogShow = true;
        }

        public void UpdateDailyKilometers()
        {
            DateTime today = DateTime.Today;
            IQueryable<Trip> temp = this.db.Trip.Where(t => t.TripEndedTime > today);
            if (temp.Any())
            {
                this.DailyKilometers = temp.Sum(t => t.Distance * t.CrewCount);
            }
        }

        public void UpdateNumBoatsOut()
        {
            this.NumBoatsOut = this.db.Trip.Count(t => t.TripEndedTime == null);
        }

        #endregion

        #region Methods

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
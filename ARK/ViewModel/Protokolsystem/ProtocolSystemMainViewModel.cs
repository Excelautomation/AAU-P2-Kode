using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Base.Interfaces.Info;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : PageContainerViewModelBase, IKeyboardContainerViewModelBase,
        IFilterContainerViewModel, IInfoContainerViewModel
    {
        #region PrivateFields

        private BeginTripBoats _beginTripBoatsPage;
        private ICommand _boatsOut;
        private BoatsOut _boatsOutPage;
        private ICommand _createDamage;
        private CreateInjury _createInjuryPage;
        private ICommand _createLongDistance;
        private CreateLongDistance _createLongDistancePage;
        private FrameworkElement _currentInfo;
        private DistanceStatistics _distanceStatisticsPage;
        private bool _enableFilters;
        private bool _enableSearch;
        private ICommand _endTrip;
        private EndTrip _endTripPage;
        private FrameworkElement _filter;
        private ObservableCollection<FrameworkElement> _filters = new ObservableCollection<FrameworkElement>();
        private string _headlineText;
        private OnScreenKeyboard _keyboard;
        private ICommand _memberInformation;
        private MembersInformation _membersInformationPage;
        private ICommand _startTrip;
        private ICommand _statisticsDistance;

        #endregion

        public ProtocolSystemMainViewModel()
        {
            TimeCounter.StartTimer();

            StartTrip.Execute(null);

            KeyboardTextChanged +=
                (sender, args) =>
                {
                    if (SearchTextChanged != null)
                        SearchTextChanged(sender, new SearchEventArgs(KeyboardText));
                };

#if !DEBUG
            LoadPages();
#endif

            TimeCounter.StopTime();
        }

        #region Pages

        public string HeadlineText
        {
            get { return _headlineText; }
            set
            {
                _headlineText = value;
                Notify();
            }
        }

        public BeginTripBoats BeginTripBoatsPage
        {
            get { return _beginTripBoatsPage ?? (_beginTripBoatsPage = new BeginTripBoats()); }
        }

        public EndTrip EndTripPage
        {
            get { return _endTripPage ?? (_endTripPage = new EndTrip()); }
        }

        public BoatsOut BoatsOutPage
        {
            get { return _boatsOutPage ?? (_boatsOutPage = new BoatsOut()); }
        }

        public DistanceStatistics DistanceStatisticsPage
        {
            get { return _distanceStatisticsPage ?? (_distanceStatisticsPage = new DistanceStatistics()); }
        }

        public MembersInformation MembersInformationPage
        {
            get { return _membersInformationPage ?? (_membersInformationPage = new MembersInformation()); }
        }

        public CreateInjury CreateInjuryPage
        {
            get { return _createInjuryPage ?? (_createInjuryPage = new CreateInjury()); }
        }

        public CreateLongDistance CreateLongDistancePage
        {
            get { return _createLongDistancePage ?? (_createLongDistancePage = new CreateLongDistance()); }
        }

        public ICommand StartTrip
        {
            get
            {
                return _startTrip ??
                       (_startTrip =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => BeginTripBoatsPage), "START ROTUR"));
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return _endTrip ??
                       (_endTrip = GetNavigateCommand(new Lazy<FrameworkElement>(() => EndTripPage), "AFSLUT ROTUR"));
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return _boatsOut ??
                       (_boatsOut =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => BoatsOutPage), "BÅDE PÅ VANDET"));
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return _statisticsDistance ??
                       (_statisticsDistance =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => DistanceStatisticsPage),
                               "KILOMETERSTATISTIK"));
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return _memberInformation ??
                       (_memberInformation =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => MembersInformationPage),
                               "MEDLEMSINFORMATION"));
            }
        }

        public ICommand CreateDamage
        {
            get
            {
                return _createDamage ??
                       (_createDamage =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => CreateInjuryPage),
                               "SKADE-BLANKET"));
            }
        }

        public ICommand CreateLongDistance
        {
            get
            {
                return _createLongDistance ??
                       (_createLongDistance =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => CreateLongDistancePage),
                               "LANGTUR-BLANKET"));
            }
        }

        public void LoadPages()
        {
            object a;
            a = BeginTripBoatsPage;
            a = EndTripPage;
            a = BoatsOutPage;
            a = DistanceStatisticsPage;
            a = MembersInformationPage;
            a = CreateInjuryPage;
            a = CreateLongDistancePage;
        }

        public override void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle)
        {
            // Deaktiver filter
            EnableSearch = false;
            EnableFilters = false;

            // Sæt filter
            var viewModelbase = page.Value.DataContext as IFilterContentViewModel;
            if (viewModelbase != null)
            {
                Filter = viewModelbase.Filter;
            }
            else
                Filter = null;

            // Skjul og clear keyboard
            KeyboardText = "";

            // Fjern info
            CurrentInfo = null;

            base.NavigateToPage(page, pageTitle);
        }

        #endregion

        #region Keyboard

        public OnScreenKeyboard Keyboard
        {
            get
            {
                if (_keyboard != null) return _keyboard;

                Keyboard = new OnScreenKeyboard();
                KeyboardHide();

                // Kalder sig selv igen for at få nyt resultat
                return Keyboard;
            }
            set
            {
                _keyboard = value;
                Notify();
            }
        }

        public bool KeyboardToggled
        {
            get { return EnableSearch; }
        }

        public void KeyboardShow()
        {
            EnableSearch = true;
        }

        public void KeyboardHide()
        {
            EnableSearch = false;
        }

        public event EventHandler KeyboardTextChanged
        {
            add { ((KeyboardViewModel) Keyboard.DataContext).TextChanged += value; }
            remove { ((KeyboardViewModel) Keyboard.DataContext).TextChanged -= value; }
        }

        public string KeyboardText
        {
            get { return ((KeyboardViewModel) Keyboard.DataContext).Text; }
            private set { ((KeyboardViewModel) Keyboard.DataContext).Text = value; }
        }

        public void KeyboardClear()
        {
            KeyboardText = "";
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

        public event EventHandler<SearchEventArgs> SearchTextChanged;
        public event EventHandler<FilterEventArgs> FilterTextChanged;

        public bool EnableSearch
        {
            get { return _enableSearch; }
            set
            {
                _enableSearch = value;
                Notify();

                // Hvis keyboard er blevet aktiveret
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
            // Tjek page
            if (infopage == null) throw new ArgumentNullException("infopage");

            // Sæt nuværende info
            CurrentInfo = infopage;

            // Tjek viewModel
            var viewModel = infopage.DataContext as IInfoContentViewModel<T>;

            // Sæt parent
            if (viewModel == null) return;

            viewModel.Parent = this;
            viewModel.Info = info;
        }

        #endregion
    }
}
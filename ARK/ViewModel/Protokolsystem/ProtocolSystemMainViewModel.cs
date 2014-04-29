using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : PageContainerViewModelBase, IKeyboardContainerViewModelBase,
        IFilterContainerViewModel, IInfoContainerViewModel
    {
        private ObservableCollection<FrameworkElement> _filters = new ObservableCollection<FrameworkElement>();
        private ICommand _boatsOut;
        private ICommand _createDamage;
        private ICommand _createLongDistance;
        private FrameworkElement _currentInfo;
        private bool _enableFilters;
        private bool _enableSearch;
        private ICommand _endTrip;
        private string _headlineText;
        private OnScreenKeyboard _keyboard;
        private bool _keyboardEnabled;
        private ICommand _memberInformation;
        private ICommand _startTrip;
        private ICommand _statisticsDistance;

        public ProtocolSystemMainViewModel()
        {
            TimeCounter.StartTimer();

            KeyboardEnabled = true;
            StartTrip.Execute(null);

            KeyboardTextChanged +=
                (sender, args) =>
                {
                    if (SearchTextChanged != null)
                        SearchTextChanged(sender, new SearchEventArgs(KeyboardText));
                };

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

        public ICommand StartTrip
        {
            get
            {
                return _startTrip ??
                       (_startTrip =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new BeginTripBoats()), "START ROTUR"));
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return _endTrip ??
                       (_endTrip = GetNavigateCommand(new Lazy<FrameworkElement>(() => new EndTrip()), "AFSLUT ROTUR"));
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return _boatsOut ??
                       (_boatsOut =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new BoatsOut()), "BÅDE PÅ VANDET"));
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return _statisticsDistance ??
                       (_statisticsDistance =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new DistanceStatistics()),
                               "KILOMETERSTATISTIK"));
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return _memberInformation ??
                       (_memberInformation =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new MembersInformation()),
                               "MEDLEMSINFORMATION"));
            }
        }

        public ICommand CreateDamage
        {
            get
            {
                return _createDamage ??
                       (_createDamage =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new CreateInjury()),
                               "OPRET BLANKET ► SKADE"));
            }
        }

        public ICommand CreateLongDistance
        {
            get
            {
                return _createLongDistance ??
                       (_createLongDistance =
                           GetNavigateCommand(new Lazy<FrameworkElement>(() => new CreateLongDistance()),
                               "OPRET BLANKET ► LANGTUR"));
            }
        }

        public override void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle)
        {
            // Deaktiver filter
            EnableSearch = false;
            EnableFilters = false;
            Filters.Clear();

            // Skjul og clear keyboard
            KeyboardHide();
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

                // Vil kalde sig selv igen
                return Keyboard;
            }
            set
            {
                _keyboard = value;
                Notify();
            }
        }

        public bool KeyboardEnabled
        {
            get { return _keyboardEnabled; }
            private set
            {
                _keyboardEnabled = value;
                if (KeyboardToggled) KeyboardHide();
                Notify();

                // Opdater onexecute changed
                var kCommand = KeyboardToggle as DelegateCommand<object>;
                if (kCommand != null)
                    kCommand.OnCanExecuteChanged();
            }
        }

        public bool KeyboardToggled
        {
            get { return Keyboard.Visibility == Visibility.Visible; }
        }

        public void KeyboardShow()
        {
            if (KeyboardEnabled)
                Keyboard.Visibility = Visibility.Visible;
            else
                KeyboardHide();

            NotifyKeyboard();
        }

        public void KeyboardHide()
        {
            Keyboard.Visibility = Visibility.Collapsed;
            NotifyKeyboard();
        }

        public void KeyboardEnable()
        {
            KeyboardEnabled = true;
        }

        public void KeyboardDisable()
        {
            KeyboardEnabled = false;
        }

        public ICommand KeyboardToggle
        {
            get
            {
                return GetCommand<object>(o =>
                {
                    if (KeyboardToggled)
                        KeyboardHide();
                    else
                        KeyboardShow();
                });
            }
        }

        private void NotifyKeyboard()
        {
            NotifyCustom("KeyboardToggled");
        }

        public event EventHandler KeyboardTextChanged
        {
            add { ((KeyboardViewModel)Keyboard.DataContext).TextChanged += value; }
            remove { ((KeyboardViewModel)Keyboard.DataContext).TextChanged -= value; }
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

        public ICommand SearchChangedCommand
        {
            get
            {
                return
                    GetCommand<string>(
                        s => { if (SearchTextChanged != null) SearchTextChanged(this, new SearchEventArgs(s)); });
            }
        }

        public event EventHandler<SearchEventArgs> SearchTextChanged;

        public bool EnableSearch
        {
            get { return _enableSearch; }
            set
            {
                _enableSearch = value;
                Notify();
            }
        }

        public bool EnableFilters
        {
            get { return _enableFilters; }
            set
            {
                _enableFilters = value;
                Notify();
            }
        }

        public ObservableCollection<FrameworkElement> Filters
        {
            get { return _filters; }
            set { _filters = value; Notify(); }
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
﻿using System;
using System.Windows;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtocolSystemMainViewModel : PageContainerViewModelBase, IKeyboardContainerViewModelBase,
        IFilterContainerViewModel, IInfoContainerViewModel
    {
        private ICommand _boatsOut;
        private ICommand _createDamage;
        private ICommand _createLongDistance;
        private FrameworkElement _currentInfo;
        private ICommand _endTrip;
        private string _headlineText;
        private OnScreenKeyboard _keyboard;
        private bool _keyboardEnabled;
        private ICommand _memberInformation;
        private ICommand _startTrip;
        private ICommand _statisticsDistance;

        public ProtocolSystemMainViewModel()
        {
            KeyboardEnabled = true;
            StartTrip.Execute(null);
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
            get { return _startTrip ?? (_startTrip = GetNavigateCommand(new Lazy<FrameworkElement>(() => new BeginTripBoats()), "Start rotur")); }
        }

        public ICommand EndTrip
        {
            get { return _endTrip ?? (_endTrip = GetNavigateCommand(new Lazy<FrameworkElement>(() => new EndTrip()), "Afslut rotur")); }
        }

        public ICommand BoatsOut
        {
            get { return _boatsOut ?? (_boatsOut = GetNavigateCommand(new Lazy<FrameworkElement>(() => new BoatsOut()), "Både på vandet")); }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return _statisticsDistance ??
                       (_statisticsDistance = GetNavigateCommand(new Lazy<FrameworkElement>(() => new DistanceStatistics()), "Kilometerstatistik"));
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return _memberInformation ??
                       (_memberInformation = GetNavigateCommand(new Lazy<FrameworkElement>(() => new MembersInformation()), "Medlemsinformation"));
            }
        }

        public ICommand CreateDamage
        {
            get
            {
                return _createDamage ?? (_createDamage = GetNavigateCommand(new Lazy<FrameworkElement>(() => new CreateInjury()), "Opret blanket ► Skade"));
            }
        }

        public ICommand CreateLongDistance
        {
            get
            {
                return _createLongDistance ??
                       (_createLongDistance = GetNavigateCommand(new Lazy<FrameworkElement>(() => new CreateLongDistance()), "Opret blanket ► Langtur"));
            }
        }

        public override void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle)
        {
            // Skjul keyboard
            KeyboardHide();

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
            Keyboard.Visibility = Visibility.Visible;
            NotifyCustom("KeyboardToggled");
        }

        public void KeyboardHide()
        {
            Keyboard.Visibility = Visibility.Collapsed;
            NotifyCustom("KeyboardToggled");
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
                }, o => KeyboardEnabled);
            }
        }

        #endregion

        #region Filter

        public void Filter()
        {
            throw new NotImplementedException();
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
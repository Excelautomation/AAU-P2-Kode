using System;
using System.Windows;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem;

namespace ARK.ViewModel.Base
{
    public class KeyboardContainerViewModelBase : PageContainerViewModelBase, IKeyboardContainerViewModelBase
    {
        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            // Hide and clear keyboard
            KeyboardText = "";
            KeyboardHide();

            base.NavigateToPage(page, pageTitle);
        }

        private OnScreenKeyboard _keyboard;
        private bool _enableKeyboard;

        public OnScreenKeyboard Keyboard
        {
            get
            {
                if (_keyboard != null) return _keyboard;

                Keyboard = new OnScreenKeyboard();
                KeyboardHide();

                return Keyboard;
            }
            set
            {
                _keyboard = value;
                Notify();
            }
        }

        public virtual ICommand KeyboardToggle
        {
            get { return GetCommand<object>(e => KeyboardToggled = !KeyboardToggled); }
        }

        public bool KeyboardToggled
        {
            get { return _enableKeyboard; }
            set
            {
                _enableKeyboard = value;
                Notify();
            }
        }

        public string KeyboardText
        {
            get { return ((KeyboardViewModel) Keyboard.DataContext).Text; }
            protected set { ((KeyboardViewModel) Keyboard.DataContext).Text = value; }
        }

        public void KeyboardShow()
        {
            KeyboardToggled = true;
        }

        public void KeyboardHide()
        {
            KeyboardToggled = false;
        }

        public event EventHandler KeyboardTextChanged
        {
            add { ((KeyboardViewModel) Keyboard.DataContext).TextChanged += value; }
            remove { ((KeyboardViewModel) Keyboard.DataContext).TextChanged -= value; }
        }

        public void KeyboardClear()
        {
            KeyboardText = "";
        }
    }
}
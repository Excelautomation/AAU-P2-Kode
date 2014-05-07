using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem;
using KeyboardEventArgs = ARK.ViewModel.Protokolsystem.KeyboardEventArgs;

namespace ARK.ViewModel.Base
{
    public class KeyboardContainerViewModelBase : PageContainerViewModelBase, IKeyboardContainerViewModelBase
    {
        private bool _enableKeyboard;
        private OnScreenKeyboard _keyboard;

        public KeyboardContainerViewModelBase()
        {
            // Setup keyboard listener
            KeyboardTextChanged += (sender, e) =>
            {
                if (CurrentSelectedTextBox == null) return;

                var textBox = CurrentSelectedTextBox as TextBox;
                if (textBox != null)
                    textBox.Text = KeyboardText;
            };
        }

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

        public ICommand KeyboardToggle
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // Change keyboard state
                    KeyboardToggled = !KeyboardToggled;

                    // Check if a textbox has been selected
                    if (CurrentSelectedTextBox == null)
                        return;

                    // Update keyboard text
                    var textbox = CurrentSelectedTextBox as TextBox;
                    if (textbox != null)
                        KeyboardText = textbox.Text;
                });
            }
        }

        private FrameworkElement CurrentSelectedTextBox { get; set; }

        public ICommand GotFocus
        {
            get { return GetCommand<FrameworkElement>(element => { CurrentSelectedTextBox = element; }); }
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

        public event EventHandler<KeyboardEventArgs> KeyboardTextChanged
        {
            add { ((KeyboardViewModel) Keyboard.DataContext).TextChanged += value; }
            remove { ((KeyboardViewModel) Keyboard.DataContext).TextChanged -= value; }
        }

        public void KeyboardClear()
        {
            KeyboardText = "";
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            // Hide and clear keyboard
            KeyboardText = "";
            KeyboardHide();

            // Remove currentselected textbox
            CurrentSelectedTextBox = null;

            base.NavigateToPage(page, pageTitle);
        }
    }
}
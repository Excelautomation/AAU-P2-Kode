using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ARK.View;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem;

using KeyboardEventArgs = ARK.ViewModel.Protokolsystem.KeyboardEventArgs;

namespace ARK.ViewModel.Base
{
    public abstract class KeyboardContainerViewModelBase : PageContainerViewModelBase, IKeyboardContainerViewModelBase
    {
        private FrameworkElement _currentSelectedTextBox;

        private bool _enableKeyboard;

        private OnScreenKeyboard _keyboard;

        public KeyboardContainerViewModelBase()
        {
            // Setup keyboard listener
            KeyboardTextChanged += (sender, e) =>
                {
                    if (CurrentSelectedTextBox == null)
                    {
                        return;
                    }

                    // Update currentselectedtextbox text
                    var textBox = CurrentSelectedTextBox as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = KeyboardText;
                    }

                    // Update passwordtext
                    if (!string.IsNullOrEmpty(KeyboardText))
                    {
                        var passwordbox = CurrentSelectedTextBox as PasswordBox;
                        if (passwordbox != null)
                        {
                            passwordbox.Password += KeyboardText;
                            KeyboardClear();
                        }
                    }
                };
        }

        public OnScreenKeyboard Keyboard
        {
            get
            {
                if (_keyboard != null)
                {
                    return _keyboard;
                }

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
                return GetCommand(
                    () =>
                        {
                            // If keyboardstate is on ignore check for currentSelectedTextBox
                            if (KeyboardToggled)
                            {
                                KeyboardToggled = false;
                                return;
                            }

                            // Check if a textbox has been selected
                            // If true change keyboard state
                            if (CurrentSelectedTextBox != null)
                            {
                                KeyboardToggled = !KeyboardToggled;
                            }
                        });
            }
        }

        private FrameworkElement CurrentSelectedTextBox
        {
            get
            {
                return _currentSelectedTextBox;
            }

            set
            {
                // Unbind event if current is not null
                if (_currentSelectedTextBox != null)
                {
                    // Handle textbox
                    var ctextbox = CurrentSelectedTextBox as TextBox;
                    if (ctextbox != null)
                    {
                        ctextbox.TextChanged -= textbox_TextChanged;
                    }
                }

                _currentSelectedTextBox = value;

                // Update keyboard text and bind event
                var textbox = CurrentSelectedTextBox as TextBox;
                if (textbox != null)
                {
                    // Set text
                    KeyboardText = textbox.Text;

                    // Bind eventhandler to support twoway
                    textbox.TextChanged += textbox_TextChanged;
                }

                // Handle passwordbox
                var passwordbox = CurrentSelectedTextBox as PasswordBox;
                if (passwordbox != null)
                {
                    KeyboardText = string.Empty;
                }
            }
        }

        public event EventHandler<KeyboardEventArgs> KeyboardTextChanged
        {
            add
            {
                ((KeyboardViewModel)Keyboard.DataContext).TextChanged += value;
            }

            remove
            {
                ((KeyboardViewModel)Keyboard.DataContext).TextChanged -= value;
            }
        }

        public ICommand GotFocus
        {
            get
            {
                return GetCommand(element =>
                {
                    KeyboardGotFocus(element);
                });
            }
        }

        protected virtual void KeyboardGotFocus(object element)
        {
            CurrentSelectedTextBox = (FrameworkElement)element;
        }

        public string KeyboardText
        {
            get
            {
                return ((KeyboardViewModel)Keyboard.DataContext).Text;
            }

            protected set
            {
                ((KeyboardViewModel)Keyboard.DataContext).Text = value;
            }
        }

        public virtual bool KeyboardToggled
        {
            get
            {
                return _enableKeyboard;
            }

            set
            {
                _enableKeyboard = value;
                Notify();
            }
        }

        public void KeyboardClear()
        {
            KeyboardText = string.Empty;
        }

        public void KeyboardHide()
        {
            KeyboardToggled = false;
        }

        public void KeyboardShow()
        {
            KeyboardToggled = true;
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            base.NavigateToPage(page, pageTitle);

            // Remove currentselected textbox
            CurrentSelectedTextBox = null;

            // Hide and clear keyboard
            KeyboardText = string.Empty;
            KeyboardHide();
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update keyboard text
            var textbox = CurrentSelectedTextBox as TextBox;
            if (textbox != null)
            {
                // Set text
                KeyboardText = textbox.Text;
            }
        }
    }
}
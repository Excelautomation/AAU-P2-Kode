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
        #region Fields

        private FrameworkElement _currentSelectedTextBox;

        private bool _enableKeyboard;

        private OnScreenKeyboard _keyboard;

        #endregion

        #region Constructors and Destructors

        public KeyboardContainerViewModelBase()
        {
            // Setup keyboard listener
            this.KeyboardTextChanged += (sender, e) =>
                {
                    if (this.CurrentSelectedTextBox == null)
                    {
                        return;
                    }

                    // Update currentselectedtextbox text
                    var textBox = this.CurrentSelectedTextBox as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = this.KeyboardText;
                    }

                    // Update passwordtext
                    if (!string.IsNullOrEmpty(this.KeyboardText))
                    {
                        var passwordbox = this.CurrentSelectedTextBox as PasswordBox;
                        if (passwordbox != null)
                        {
                            passwordbox.Password += this.KeyboardText;
                            this.KeyboardClear();
                        }
                    }
                };
        }

        #endregion

        #region Public Events

        public event EventHandler<KeyboardEventArgs> KeyboardTextChanged
        {
            add
            {
                ((KeyboardViewModel)this.Keyboard.DataContext).TextChanged += value;
            }

            remove
            {
                ((KeyboardViewModel)this.Keyboard.DataContext).TextChanged -= value;
            }
        }

        #endregion

        #region Public Properties

        public ICommand GotFocus
        {
            get
            {
                return this.GetCommand(element => { this.CurrentSelectedTextBox = (FrameworkElement)element; });
            }
        }

        public OnScreenKeyboard Keyboard
        {
            get
            {
                if (this._keyboard != null)
                {
                    return this._keyboard;
                }

                this.Keyboard = new OnScreenKeyboard();
                this.KeyboardHide();

                return this.Keyboard;
            }

            set
            {
                this._keyboard = value;
                this.Notify();
            }
        }

        public string KeyboardText
        {
            get
            {
                return ((KeyboardViewModel)this.Keyboard.DataContext).Text;
            }

            protected set
            {
                ((KeyboardViewModel)this.Keyboard.DataContext).Text = value;
            }
        }

        public ICommand KeyboardToggle
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // If keyboardstate is on ignore check for currentSelectedTextBox
                            if (this.KeyboardToggled)
                            {
                                this.KeyboardToggled = false;
                                return;
                            }

                            // Check if a textbox has been selected
                            // If true change keyboard state
                            if (this.CurrentSelectedTextBox != null)
                            {
                                this.KeyboardToggled = !this.KeyboardToggled;
                            }
                        });
            }
        }

        public bool KeyboardToggled
        {
            get
            {
                return this._enableKeyboard;
            }

            set
            {
                this._enableKeyboard = value;
                this.Notify();
            }
        }

        #endregion

        #region Properties

        private FrameworkElement CurrentSelectedTextBox
        {
            get
            {
                return this._currentSelectedTextBox;
            }

            set
            {
                // Unbind event if current is not null
                if (this._currentSelectedTextBox != null)
                {
                    // Handle textbox
                    var ctextbox = this.CurrentSelectedTextBox as TextBox;
                    if (ctextbox != null)
                    {
                        ctextbox.TextChanged -= this.textbox_TextChanged;
                    }
                }

                this._currentSelectedTextBox = value;

                // Update keyboard text and bind event
                var textbox = this.CurrentSelectedTextBox as TextBox;
                if (textbox != null)
                {
                    // Set text
                    this.KeyboardText = textbox.Text;

                    // Bind eventhandler to support twoway
                    textbox.TextChanged += this.textbox_TextChanged;
                }

                // Handle passwordbox
                var passwordbox = this.CurrentSelectedTextBox as PasswordBox;
                if (passwordbox != null)
                {
                    this.KeyboardText = string.Empty;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        public void KeyboardClear()
        {
            this.KeyboardText = string.Empty;
        }

        public void KeyboardHide()
        {
            this.KeyboardToggled = false;
        }

        public void KeyboardShow()
        {
            this.KeyboardToggled = true;
        }

        public override void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            base.NavigateToPage(page, pageTitle);

            // Remove currentselected textbox
            this.CurrentSelectedTextBox = null;

            // Hide and clear keyboard
            this.KeyboardText = string.Empty;
            this.KeyboardHide();
        }

        #endregion

        #region Methods

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update keyboard text
            var textbox = this.CurrentSelectedTextBox as TextBox;
            if (textbox != null)
            {
                // Set text
                this.KeyboardText = textbox.Text;
            }
        }

        #endregion
    }
}
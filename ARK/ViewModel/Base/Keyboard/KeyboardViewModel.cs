using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;

namespace ARK.ViewModel.Base.Keyboard
{
    public abstract class KeyboardViewModel : ViewModel
    {
        private OnScreenKeyboard _keyboard;
        private bool _keyboardEnabled = true;

        public KeyboardViewModel()
        {
            var binding = new CommandBinding(GlobalCommands.ToggleKeyboard, Toggle, CanToggle);

            // Register CommandBinding for all windows.
            CommandManager.RegisterClassCommandBinding(typeof(UserControl), binding);
        }

        public OnScreenKeyboard Keyboard
        {
            get
            {
                if (_keyboard != null) return _keyboard;

                Keyboard = new OnScreenKeyboard();
                HideKeyboard();

                return Keyboard;
            }
            protected set
            {
                _keyboard = value;
                Notify();
            }
        }

        private void Toggle(object sender, ExecutedRoutedEventArgs e)
        {
            if (KeyboardToggled)
            {
                HideKeyboard();
            }
            else
            {
                ShowKeyboard();
            }

            var contentControl = sender as ContentControl;
            if (contentControl == null) return;

            var dataContext = contentControl.DataContext as IKeyboardChange;
            if (dataContext == null) return;
            
            dataContext.KeyboardToggleText = KeyboardToggled ? "SKJUL\nTASTATUR" : "VIS\nTASTATUR";
        }

        private void CanToggle(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = KeyboardEnabled;
        }

        public bool KeyboardToggled
        {
            get { return Keyboard.Visibility == Visibility.Visible; }
        }

        public bool KeyboardEnabled
        {
            get { return _keyboardEnabled; }
            set
            {
                _keyboardEnabled = value;
                Notify();
            }
        }

        protected void ShowKeyboard()
        {
            Keyboard.Visibility = Visibility.Visible;
        }

        protected void HideKeyboard()
        {
            Keyboard.Visibility = Visibility.Collapsed;
        }
    }
}
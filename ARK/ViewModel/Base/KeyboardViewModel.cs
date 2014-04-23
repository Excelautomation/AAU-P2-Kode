using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public abstract class KeyboardViewModel : ViewModel
    {
        private bool _keyboardToggled;
        private bool _keyboardEnabled;

        public bool KeyboardToggled
        {
            get { return _keyboardToggled; }
            set
            {
                _keyboardToggled = value;
                Notify();
            }
        }

        public bool KeyboardEnabled
        {
            get { return _keyboardEnabled; }
            set { _keyboardEnabled = value; Notify(); }
        }

        public ICommand ToggleKeyboard
        {
            get
            {
                return GetCommand<bool>(e =>
                {
                    if (KeyboardToggled)
                        HideKeyboard();
                    else
                        ShowKeyboard();
                }, e => KeyboardEnabled);
            }
        }

        protected abstract void ShowKeyboard();
        protected abstract void HideKeyboard();
    }
}
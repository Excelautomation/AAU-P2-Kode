using System.Windows.Input;

namespace ARK.ViewModel.Base.Keyboard
{
    public abstract class KeyboardRelayViewModel : ViewModel, IKeyboardViewModel
    {
        private IKeyboardViewModel _keyboardViewModel;

        protected KeyboardRelayViewModel(IKeyboardViewModel parentViewModel)
        {
            KeyboardViewModel = parentViewModel;
        }

        public IKeyboardViewModel KeyboardViewModel
        {
            get { return _keyboardViewModel; }
            set
            {
                _keyboardViewModel = value;
                Notify();
            }
        }

        public string KeyboardText
        {
            get { return KeyboardViewModel.KeyboardText; }
            set { KeyboardViewModel.KeyboardText = value; }
        }

        public bool KeyboardToggled
        {
            get { return KeyboardViewModel.KeyboardToggled; }
            set { KeyboardViewModel.KeyboardToggled = value; }
        }

        public bool KeyboardEnabled
        {
            get { return KeyboardViewModel.KeyboardEnabled; }
            set { KeyboardViewModel.KeyboardEnabled = value; }
        }

        public ICommand ToggleKeyboard
        {
            get { return KeyboardViewModel.ToggleKeyboard; }
        }
    }
}
using System.Windows.Input;

namespace ARK.ViewModel.Base.Keyboard
{
    public interface IKeyboardViewModel
    {
        string KeyboardText { get; set; }
        bool KeyboardToggled { get; set; }
        bool KeyboardEnabled { get; set; }
        ICommand ToggleKeyboard { get; }
    }
}
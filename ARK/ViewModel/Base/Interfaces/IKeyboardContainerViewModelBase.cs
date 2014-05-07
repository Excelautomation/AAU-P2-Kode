using System;
using System.Windows.Input;
using KeyboardEventArgs = ARK.ViewModel.Protokolsystem.KeyboardEventArgs;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        string KeyboardText { get; }

        ICommand GotFocus { get; }
        bool KeyboardToggled { get; }
        event EventHandler<KeyboardEventArgs> KeyboardTextChanged;
        void KeyboardClear();

        void KeyboardShow();
        void KeyboardHide();
    }
}
using System;
using System.Windows.Input;

using KeyboardEventArgs = ARK.ViewModel.Protokolsystem.KeyboardEventArgs;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IPageContainerViewModelBase
    {
        ICommand GotFocus { get; }

        string KeyboardText { get; }

        bool KeyboardToggled { get; }

        event EventHandler<KeyboardEventArgs> KeyboardTextChanged;

        void KeyboardClear();

        void KeyboardHide();

        void KeyboardShow();
    }
}
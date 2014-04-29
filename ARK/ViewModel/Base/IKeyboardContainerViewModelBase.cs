using System;
using System.Windows.Input;

namespace ARK.ViewModel.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        event EventHandler KeyboardTextChanged;
        string KeyboardText { get; }
        void KeyboardClear();

        bool KeyboardEnabled { get; }
        bool KeyboardToggled { get; }
        ICommand KeyboardToggle { get; }

        void KeyboardShow();
        void KeyboardHide();
        void KeyboardEnable();
        void KeyboardDisable();
    }
}
using System;
using System.Windows.Input;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        event EventHandler KeyboardTextChanged;
        string KeyboardText { get; }
        void KeyboardClear();

        bool KeyboardToggled { get; }
        ICommand KeyboardToggle { get; }

        void KeyboardShow();
        void KeyboardHide();
    }
}
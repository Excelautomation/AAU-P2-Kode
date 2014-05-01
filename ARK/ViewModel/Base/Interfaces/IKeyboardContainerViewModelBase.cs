using System;
using System.Windows.Input;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        string KeyboardText { get; }

        bool KeyboardToggled { get; }
        event EventHandler KeyboardTextChanged;
        void KeyboardClear();

        void KeyboardShow();
        void KeyboardHide();
    }
}
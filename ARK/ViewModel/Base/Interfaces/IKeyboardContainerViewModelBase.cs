using System;
using System.Windows.Input;

using KeyboardEventArgs = ARK.ViewModel.Protokolsystem.KeyboardEventArgs;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        #region Public Events

        event EventHandler<KeyboardEventArgs> KeyboardTextChanged;

        #endregion

        #region Public Properties

        ICommand GotFocus { get; }

        string KeyboardText { get; }

        bool KeyboardToggled { get; }

        #endregion

        #region Public Methods and Operators

        void KeyboardClear();

        void KeyboardHide();

        void KeyboardShow();

        #endregion
    }
}
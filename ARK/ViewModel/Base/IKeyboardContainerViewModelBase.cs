using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        bool KeyboardEnabled { get; }
        bool KeyboardToggled { get; }
        ICommand KeyboardToggle { get; }

        void KeyboardShow();
        void KeyboardHide();
        void KeyboardEnable();
        void KeyboardDisable();
    }
}
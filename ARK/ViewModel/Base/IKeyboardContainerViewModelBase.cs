using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public interface IKeyboardContainerViewModelBase : IViewModelBase
    {
        bool KeyboardEnabled { get; }
        bool KeyboardToggled { get; }

        void KeyboardShow();
        void KeyboardHide();
        void KeyboardEnable();
        void KeyboardDisable();

        ICommand KeyboardToggle { get; }
    }
}

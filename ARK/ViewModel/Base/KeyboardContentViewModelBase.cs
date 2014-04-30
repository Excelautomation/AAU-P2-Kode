using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class KeyboardContentViewModelBase : ContentViewModelBase
    {
        public IKeyboardContainerViewModelBase Keyboard
        {
            get { return Parent as IKeyboardContainerViewModelBase; }
        }
    }
}
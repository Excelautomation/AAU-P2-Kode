using ARK.ViewModel.Interfaces;

namespace ARK.ViewModel
{
    public abstract class KeyboardContentViewModelBase : ContentViewModelBase
    {
        public IKeyboardContainerViewModelBase Keyboard
        {
            get { return Parent as IKeyboardContainerViewModelBase; }
        }
    }
}
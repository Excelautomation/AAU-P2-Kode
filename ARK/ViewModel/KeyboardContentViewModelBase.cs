using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public abstract class KeyboardContentViewModelBase : ContentViewModelBase
    {
        public IKeyboardContainerViewModelBase Keyboard
        {
            get
            {
                return Parent as IKeyboardContainerViewModelBase;
            }
        }
    }
}

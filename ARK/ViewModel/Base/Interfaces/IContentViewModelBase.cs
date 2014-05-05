using System;
using System.Windows.Input;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IContentViewModelBase : IViewModelBase
    {
        ICommand GotFocus { get; }
        IViewModelBase Parent { get; set; }
        event EventHandler ParentAttached;
        void ParentDetached();
    }
}
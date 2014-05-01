using System;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IContentViewModelBase : IViewModelBase
    {
        IViewModelBase Parent { get; set; }
        event EventHandler ParentAttached;
        void ParentDetached();
    }
}
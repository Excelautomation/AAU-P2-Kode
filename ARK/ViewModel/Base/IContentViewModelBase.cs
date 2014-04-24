using System;

namespace ARK.ViewModel.Base
{
    public interface IContentViewModelBase : IViewModelBase
    {
        IViewModelBase Parent { get; set; }
        event EventHandler ParentAttached;
    }
}
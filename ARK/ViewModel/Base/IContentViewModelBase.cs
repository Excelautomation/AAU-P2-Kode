using System;

namespace ARK.ViewModel.Interfaces
{
    public interface IContentViewModelBase : IViewModelBase
    {
        IViewModelBase Parent { get; set; }
        event EventHandler ParentAttached;
    }
}
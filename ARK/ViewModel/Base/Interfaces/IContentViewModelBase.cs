using System;
using System.Windows.Input;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IContentViewModelBase : IViewModelBase
    {
        #region Public Events

        event EventHandler ParentAttached;

        event EventHandler ParentDetached;

        #endregion

        #region Public Properties

        ICommand GotFocus { get; }

        IViewModelBase Parent { get; set; }

        #endregion
    }
}
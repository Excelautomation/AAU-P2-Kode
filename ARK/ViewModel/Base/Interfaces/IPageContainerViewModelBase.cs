using System;
using System.Windows;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IPageContainerViewModelBase : IViewModelBase
    {
        #region Public Properties

        FrameworkElement CurrentPage { get; }

        string CurrentPageTitle { get; }

        #endregion

        #region Public Methods and Operators

        void NavigateToPage(Func<FrameworkElement> page, string pageTitle);

        #endregion
    }
}
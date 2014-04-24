using System;
using System.Windows;

namespace ARK.ViewModel.Base
{
    public interface IPageContainerViewModelBase : IViewModelBase
    {
        string CurrentPageTitle { get; }
        FrameworkElement CurrentPage { get; }

        void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle);

    }
}

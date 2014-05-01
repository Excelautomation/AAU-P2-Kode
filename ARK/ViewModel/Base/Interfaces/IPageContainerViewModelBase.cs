using System;
using System.Windows;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IPageContainerViewModelBase : IViewModelBase
    {
        string CurrentPageTitle { get; }
        FrameworkElement CurrentPage { get; }

        void NavigateToPage(Func<FrameworkElement> page, string pageTitle);
    }
}
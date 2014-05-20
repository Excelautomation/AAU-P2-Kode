using System;
using System.Windows;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IPageContainerViewModelBase : IViewModelBase
    {
        FrameworkElement CurrentPage { get; }

        string CurrentPageTitle { get; }

        void NavigateToPage(Func<FrameworkElement> page, string pageTitle);
    }
}
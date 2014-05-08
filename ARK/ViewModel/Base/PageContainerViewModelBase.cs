using System;
using System.Windows;
using System.Windows.Input;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class PageContainerViewModelBase : ViewModelBase, IPageContainerViewModelBase
    {
        private FrameworkElement _currentPage;
        private string _currentPageTitle;

        public FrameworkElement CurrentPage
        {
            get { return _currentPage; }
            protected set
            {
                _currentPage = value;
                Notify();
            }
        }

        public string CurrentPageTitle
        {
            get { return _currentPageTitle; }
            protected set
            {
                _currentPageTitle = value;
                Notify();
            }
        }

        public virtual void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            // Check page
            if (page == null) throw new ArgumentNullException("page");

            // Check if currentpage is null, if cond. is true then cleanup
            if (CurrentPage != null)
            {
                var vm = CurrentPage.DataContext as IContentViewModelBase;
                if (vm != null) vm.Parent = null;
            }

            // Set current page and text
            CurrentPage = page();
            CurrentPageTitle = pageTitle;

            // Check viewModel
            var viewModel = CurrentPage.DataContext as IContentViewModelBase;

            // Set parent
            if (viewModel != null) viewModel.Parent = this;
        }

        public ICommand GetNavigateCommand(Func<FrameworkElement> page, string pageTitle)
        {
            return GetCommand<object>(obj => NavigateToPage(page, pageTitle));
        }
    }
}
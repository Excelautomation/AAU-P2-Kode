using System;
using System.Windows;
using System.Windows.Input;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
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

        public virtual void NavigateToPage(Lazy<FrameworkElement> page, string pageTitle)
        {
            // Tjek page
            if (page == null) throw new ArgumentNullException("page");

            // Sæt nuværende side og tekst
            CurrentPage = page.Value;
            CurrentPageTitle = pageTitle;

            // Tjek viewModel
            var viewModel = CurrentPage.DataContext as IContentViewModelBase;

            // Sæt parent
            if (viewModel != null) viewModel.Parent = this;
        }

        protected ICommand GetNavigateCommand(Lazy<FrameworkElement> page, string pageTitle)
        {
            return GetCommand<object>(obj => NavigateToPage(page, pageTitle));
        }
    }
}
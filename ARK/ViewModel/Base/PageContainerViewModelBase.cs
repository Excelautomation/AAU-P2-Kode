using System;
using System.Windows;
using System.Windows.Input;

using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class PageContainerViewModelBase : ViewModelBase, IPageContainerViewModelBase
    {
        #region Fields

        private FrameworkElement _currentPage;

        private string _currentPageTitle;

        #endregion

        #region Public Properties

        public FrameworkElement CurrentPage
        {
            get
            {
                return this._currentPage;
            }

            protected set
            {
                this._currentPage = value;
                this.Notify();
            }
        }

        public string CurrentPageTitle
        {
            get
            {
                return this._currentPageTitle;
            }

            protected set
            {
                this._currentPageTitle = value;
                this.Notify();
            }
        }

        #endregion

        #region Public Methods and Operators

        public ICommand GetNavigateCommand(Func<FrameworkElement> page, string pageTitle)
        {
            return this.GetCommand(() => this.NavigateToPage(page, pageTitle));
        }

        public virtual void NavigateToPage(Func<FrameworkElement> page, string pageTitle)
        {
            // Check page
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            // Check if currentpage is null, if cond. is true then cleanup
            if (this.CurrentPage != null)
            {
                var vm = this.CurrentPage.DataContext as IContentViewModelBase;
                if (vm != null)
                {
                    vm.Parent = null;
                }
            }

            // Set current page and text
            this.CurrentPage = page();
            this.CurrentPageTitle = pageTitle;

            // Check viewModel
            var viewModel = this.CurrentPage.DataContext as IContentViewModelBase;

            // Set parent
            if (viewModel != null)
            {
                viewModel.Parent = this;
            }
        }

        #endregion
    }
}
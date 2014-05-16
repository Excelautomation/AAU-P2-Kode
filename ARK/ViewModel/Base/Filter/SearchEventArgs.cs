using System;

namespace ARK.ViewModel.Base.Filter
{
    public class SearchEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public SearchEventArgs(string searchText)
        {
            this.SearchText = searchText;
        }

        #endregion

        #region Public Properties

        public string SearchText { get; private set; }

        #endregion
    }
}
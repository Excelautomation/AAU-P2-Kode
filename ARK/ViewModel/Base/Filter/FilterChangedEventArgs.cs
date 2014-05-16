using System;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public FilterChangedEventArgs(FilterEventArgs filterEventArgs, SearchEventArgs searchEventArgs)
        {
            this.SearchEventArgs = searchEventArgs;
            this.FilterEventArgs = filterEventArgs;
        }

        #endregion

        #region Public Properties

        public FilterEventArgs FilterEventArgs { get; private set; }

        public SearchEventArgs SearchEventArgs { get; private set; }

        #endregion
    }
}
using System;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterChangedEventArgs : EventArgs
    {
        public FilterChangedEventArgs(FilterEventArgs filterEventArgs, SearchEventArgs searchEventArgs)
        {
            SearchEventArgs = searchEventArgs;
            FilterEventArgs = filterEventArgs;
        }

        public FilterEventArgs FilterEventArgs { get; private set; }

        public SearchEventArgs SearchEventArgs { get; private set; }
    }
}
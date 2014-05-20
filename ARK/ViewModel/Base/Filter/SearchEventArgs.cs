using System;

namespace ARK.ViewModel.Base.Filter
{
    public class SearchEventArgs : EventArgs
    {
        public SearchEventArgs(string searchText)
        {
            SearchText = searchText;
        }

        public string SearchText { get; private set; }
    }
}
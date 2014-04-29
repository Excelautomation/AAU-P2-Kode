using System;
using System.Collections.Generic;

namespace ARK.ViewModel.Filter
{
    public class FilterEventArgs : EventArgs
    {
        public string SearchText { get; private set; }
        public IEnumerable<string> Filters { get; private set; }

        public FilterEventArgs(string searchText, IEnumerable<string> filtersActive)
        {
            SearchText = searchText;
            Filters = filtersActive;
        }
    }
}
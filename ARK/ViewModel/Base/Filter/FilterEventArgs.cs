using System;
using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterEventArgs : EventArgs
    {
        public FilterEventArgs(IEnumerable<Filter> filtersActive)
        {
            Filters = filtersActive;
        }

        public IEnumerable<Filter> Filters { get; private set; }
    }
}
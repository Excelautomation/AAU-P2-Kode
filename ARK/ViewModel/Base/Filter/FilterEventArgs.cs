using System;
using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterEventArgs : EventArgs
    {
        public FilterEventArgs(IEnumerable<IFilter> filtersActive)
        {
            Filters = filtersActive;
        }

        public IEnumerable<IFilter> Filters { get; private set; }
    }
}
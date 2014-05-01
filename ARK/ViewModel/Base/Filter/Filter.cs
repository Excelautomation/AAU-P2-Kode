using System;
using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public abstract class Filter
    {
        public abstract IEnumerable<T> FilterItems<T>(IEnumerable<T> items);
        public abstract bool CanFilter<T>(IEnumerable<T> items);
    }
}
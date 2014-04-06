using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.ViewModel.Filter
{
    interface IFilter<T>
    {
        bool Active();
        bool FilterItem(T item);
        IEnumerable<T> FilterItems(T[] items);
    }
}
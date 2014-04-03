using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK.ViewModel.Filter
{
    interface IFilter<T> : ICollection<IFilterItems<T>>
    {
        bool Active();
        bool FilterItem(T item);
        IEnumerable<T> FilterItems(T[] items);
    }
}
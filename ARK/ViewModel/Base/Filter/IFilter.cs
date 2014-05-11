using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public interface IFilter
    {
        IEnumerable<T> FilterItems<T>(IEnumerable<T> items);
    }
}
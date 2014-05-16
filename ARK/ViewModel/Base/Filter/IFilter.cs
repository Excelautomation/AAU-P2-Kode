using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public interface IFilter
    {
        #region Public Methods and Operators

        IEnumerable<T> FilterItems<T>(IEnumerable<T> items);

        #endregion
    }
}
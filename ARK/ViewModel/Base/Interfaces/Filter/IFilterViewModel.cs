using System;

using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterViewModel
    {
        #region Public Events

        event EventHandler<FilterEventArgs> FilterChanged;

        #endregion
    }
}
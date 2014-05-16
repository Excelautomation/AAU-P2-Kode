using System;

using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterContainerViewModel : IViewModelBase
    {
        #region Public Events

        event EventHandler<FilterEventArgs> FilterTextChanged;

        event EventHandler<SearchEventArgs> SearchTextChanged;

        #endregion

        #region Public Properties

        bool EnableFilters { get; set; }

        bool EnableSearch { get; set; }

        #endregion
    }
}
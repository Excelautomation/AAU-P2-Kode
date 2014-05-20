using System;

using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterContainerViewModel : IViewModelBase
    {
        bool EnableFilters { get; set; }

        bool EnableSearch { get; set; }

        event EventHandler<FilterEventArgs> FilterTextChanged;

        event EventHandler<SearchEventArgs> SearchTextChanged;
    }
}
using System;
using System.Collections.ObjectModel;
using System.Windows;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterContainerViewModel : IViewModelBase
    {
        bool EnableSearch { get; set; }
        bool EnableFilters { get; set; }

        event EventHandler<FilterEventArgs> FilterTextChanged;
        event EventHandler<SearchEventArgs> SearchTextChanged;
    }
}
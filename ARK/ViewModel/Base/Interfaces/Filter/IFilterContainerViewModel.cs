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

        ObservableCollection<FrameworkElement> Filters { get; set; }
        event EventHandler<SearchEventArgs> SearchTextChanged;
    }
}
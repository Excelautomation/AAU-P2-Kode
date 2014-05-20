using System;

using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterViewModel
    {
        event EventHandler<FilterEventArgs> FilterChanged;
    }
}
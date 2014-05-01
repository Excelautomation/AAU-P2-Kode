using System;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class DistanceStatisticsFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public event EventHandler<FilterEventArgs> FilterChanged;
    }
}
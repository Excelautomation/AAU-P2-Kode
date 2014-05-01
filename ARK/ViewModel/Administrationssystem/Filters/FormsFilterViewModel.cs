using System;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class FormsFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public event EventHandler<FilterEventArgs> FilterChanged;
    }
}
using System;
using System.Collections.Generic;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Base
{
    public abstract class FilterViewModelBase : ViewModelBase, IFilterViewModel
    {
        public event EventHandler<FilterEventArgs> FilterChanged
        {
            add
            {
                FilterChangedInternal += value;
                value(this, new FilterEventArgs(GetFilter()));
            }
            remove { FilterChangedInternal -= value; }
        }

        public abstract IEnumerable<Filter.Filter> GetFilter();

        private event EventHandler<FilterEventArgs> FilterChangedInternal;

        protected virtual void OnFilterChanged()
        {
            OnFilterChanged(new FilterEventArgs(GetFilter()));
        }

        protected virtual void OnFilterChanged(FilterEventArgs e)
        {
            EventHandler<FilterEventArgs> handler = FilterChangedInternal;
            if (handler != null) handler(this, e);
        }
    }
}
using System;
using System.Collections.Generic;

using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Base
{
    public abstract class FilterViewModelBase : ViewModelBase, IFilterViewModel
    {
        #region Public Events

        public event EventHandler<FilterEventArgs> FilterChanged
        {
            add
            {
                this.FilterChangedInternal += value;
                value(this, new FilterEventArgs(this.GetFilter()));
            }

            remove
            {
                this.FilterChangedInternal -= value;
            }
        }

        #endregion

        #region Events

        private event EventHandler<FilterEventArgs> FilterChangedInternal;

        #endregion

        #region Public Methods and Operators

        public abstract IEnumerable<IFilter> GetFilter();

        #endregion

        #region Methods

        protected virtual void OnFilterChanged()
        {
            this.OnFilterChanged(new FilterEventArgs(this.GetFilter()));
        }

        protected virtual void OnFilterChanged(FilterEventArgs e)
        {
            EventHandler<FilterEventArgs> handler = this.FilterChangedInternal;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public FilterEventArgs(IEnumerable<IFilter> filtersActive)
        {
            this.Filters = filtersActive;
        }

        #endregion

        #region Public Properties

        public IEnumerable<IFilter> Filters { get; private set; }

        #endregion
    }
}
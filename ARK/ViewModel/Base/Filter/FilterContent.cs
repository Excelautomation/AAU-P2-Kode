using System;
using System.Collections.Generic;
using System.Linq;

using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterContent : ViewModelBase
    {
        #region Fields

        private IContentViewModelBase _contentViewModel;

        #endregion

        #region Constructors and Destructors

        public FilterContent(IContentViewModelBase contentViewModel)
        {
            this.ContentViewModel = contentViewModel;
        }

        #endregion

        #region Public Events

        public event EventHandler<FilterChangedEventArgs> FilterChanged;

        #endregion

        #region Public Properties

        public IContentViewModelBase ContentViewModel
        {
            get
            {
                return this._contentViewModel;
            }

            set
            {
                this._contentViewModel = value;
                this.Notify();
            }
        }

        #endregion

        #region Properties

        private IFilterContainerViewModel FilterContainer
        {
            get
            {
                return this.ContentViewModel.Parent as IFilterContainerViewModel;
            }
        }

        private FilterEventArgs LastFilterEventArgs { get; set; }

        private SearchEventArgs LastSearchEventArgs { get; set; }

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<T> FilterItems<T>(
            IEnumerable<T> items, 
            FilterEventArgs filterEventArgs, 
            bool merge = false)
        {
            List<IFilter> filters = filterEventArgs.Filters.Where(filter => filter != null).ToList();

            if (!filters.Any())
            {
                return items;
            }

            IEnumerable<T> allItems = items.ToList();
            IEnumerable<T> output = new List<T>(allItems);

            foreach (IFilter filter in filters)
            {
                output =
                    (merge ? MergeLists(filter.FilterItems(allItems), output) : filter.FilterItems(output)).ToList();
            }

            return output;
        }

        public static IEnumerable<T> MergeLists<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var outputList = new List<T>(list1);

            foreach (T elm in list2)
            {
                if (!outputList.Any(e => e.Equals(elm)))
                {
                    outputList.Add(elm);
                }
            }

            return outputList.ToList();
        }

        public void EnableFilter(bool enableSearch, bool enableFilters)
        {
            this.ContentViewModel.ParentAttached += (sender, args) =>
                {
                    // Filter
                    this.FilterContainer.EnableSearch = enableSearch;
                    this.FilterContainer.EnableFilters = enableFilters;

                    // Bind events
                    this.FilterContainer.SearchTextChanged += this.FilterContainerOnSearchTextChanged;
                    this.FilterContainer.FilterTextChanged += this.FilterContainerOnFilterTextChanged;
                };

            this.ContentViewModel.ParentDetached += (sender, args) =>
                {
                    // Unbind events
                    this.FilterContainer.SearchTextChanged -= this.FilterContainerOnSearchTextChanged;
                    this.FilterContainer.FilterTextChanged -= this.FilterContainerOnFilterTextChanged;

                    // Delete last eventargs
                    this.LastSearchEventArgs = null;
                    this.LastFilterEventArgs = null;
                };
        }

        public void UpdateFilter()
        {
            this.OnFilterChanged();
        }

        #endregion

        #region Methods

        private void FilterContainerOnFilterTextChanged(object sender, FilterEventArgs e)
        {
            this.LastFilterEventArgs = e;

            this.OnFilterChanged();
        }

        private void FilterContainerOnSearchTextChanged(object sender, SearchEventArgs e)
        {
            this.LastSearchEventArgs = e;

            this.OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            if (this.FilterChanged != null)
            {
                this.FilterChanged(this, new FilterChangedEventArgs(this.LastFilterEventArgs, this.LastSearchEventArgs));
            }
        }

        #endregion
    }
}
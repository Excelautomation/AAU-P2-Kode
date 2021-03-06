﻿using System;
using System.Collections.Generic;
using System.Linq;

using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Base.Filter
{
    public class FilterContent : ViewModelBase
    {
        private IContentViewModelBase _contentViewModel;

        public FilterContent(IContentViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;
        }

        public IContentViewModelBase ContentViewModel
        {
            get
            {
                return _contentViewModel;
            }

            set
            {
                _contentViewModel = value;
                Notify();
            }
        }

        private IFilterContainerViewModel FilterContainer
        {
            get
            {
                return ContentViewModel.Parent as IFilterContainerViewModel;
            }
        }

        private FilterEventArgs LastFilterEventArgs { get; set; }

        private SearchEventArgs LastSearchEventArgs { get; set; }

        public event EventHandler<FilterChangedEventArgs> FilterChanged;

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
            ContentViewModel.ParentAttached += (sender, args) =>
                {
                    // Filter
                    FilterContainer.EnableSearch = enableSearch;
                    FilterContainer.EnableFilters = enableFilters;

                    // Bind events
                    FilterContainer.SearchTextChanged += FilterContainerOnSearchTextChanged;
                    FilterContainer.FilterTextChanged += FilterContainerOnFilterTextChanged;
                };

            ContentViewModel.ParentDetached += (sender, args) =>
                {
                    // Unbind events
                    FilterContainer.SearchTextChanged -= FilterContainerOnSearchTextChanged;
                    FilterContainer.FilterTextChanged -= FilterContainerOnFilterTextChanged;

                    // Delete last eventargs
                    LastSearchEventArgs = null;
                    LastFilterEventArgs = null;
                };
        }

        public void UpdateFilter()
        {
            OnFilterChanged();
        }

        private void FilterContainerOnFilterTextChanged(object sender, FilterEventArgs e)
        {
            LastFilterEventArgs = e;

            OnFilterChanged();
        }

        private void FilterContainerOnSearchTextChanged(object sender, SearchEventArgs e)
        {
            LastSearchEventArgs = e;

            OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            if (FilterChanged != null)
            {
                FilterChanged(this, new FilterChangedEventArgs(LastFilterEventArgs, LastSearchEventArgs));
            }
        }
    }
}
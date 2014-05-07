using System;
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
            get { return _contentViewModel; }
            set
            {
                _contentViewModel = value;
                Notify();
            }
        }

        private IFilterContainerViewModel FilterContainer
        {
            get { return ContentViewModel.Parent as IFilterContainerViewModel; }
        }

        private SearchEventArgs LastSearchEventArgs { get; set; }
        private FilterEventArgs LastFilterEventArgs { get; set; }

        public void EnableFilter(bool enableSearch, bool enableFilters)
        {
            IFilterContainerViewModel parentFilterContainer = null;

            ContentViewModel.ParentAttached += (sender, args) =>
            {
                // Filter
                FilterContainer.EnableSearch = enableSearch;
                FilterContainer.EnableFilters = enableFilters;

                // Unbind events
                if (parentFilterContainer != null)
                {
                    parentFilterContainer.SearchTextChanged -= FilterContainerOnSearchTextChanged;
                    parentFilterContainer.FilterTextChanged -= FilterContainerOnFilterTextChanged;
                }

                // Bind events
                FilterContainer.SearchTextChanged += FilterContainerOnSearchTextChanged;
                FilterContainer.FilterTextChanged += FilterContainerOnFilterTextChanged;

                // Delete last eventargs
                LastSearchEventArgs = null;
                LastFilterEventArgs = null;

                // Set parentFilterContainer so we can unbind events in the future
                parentFilterContainer = FilterContainer;
            };
        }

        public void UpdateFilter()
        {
            OnFilterChanged();
        }

        private void FilterContainerOnSearchTextChanged(object sender, SearchEventArgs e)
        {
            LastSearchEventArgs = e;

            OnFilterChanged();
        }

        private void FilterContainerOnFilterTextChanged(object sender, FilterEventArgs e)
        {
            LastFilterEventArgs = e;

            OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterChangedEventArgs(LastFilterEventArgs, LastSearchEventArgs));
        }

        public event EventHandler<FilterChangedEventArgs> FilterChanged;

        public static IEnumerable<T> FilterItems<T>(IEnumerable<T> items, FilterEventArgs filterEventArgs)
        {
            var filters = filterEventArgs.Filters.Where(filter => filter != null).ToList();

            if (!filters.Any())
                return items;

            IEnumerable<T> output = new List<T>();
            IEnumerable<T> allItems = items.ToList();

            foreach (var filter in filters)
                output = MergeLists<T>(filter.FilterItems<T>(allItems), output);

            return output;
        } 

        public static IEnumerable<T> MergeLists<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var outputList = new List<T>(list1);

            foreach (T elm in list2)
                if (!outputList.Any(e => e.Equals(elm)))
                    outputList.Add(elm);

            return outputList.ToList();
        }
    }
}
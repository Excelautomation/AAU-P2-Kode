using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public IEnumerable<FrameworkElement> Filters
        {
            get { return FilterContainer.Filters; }
            set
            {
                if (value == null)
                    FilterContainer.Filters = new ObservableCollection<FrameworkElement>();
                else
                    FilterContainer.Filters = new ObservableCollection<FrameworkElement>(value);
            }
        }

        public void EnableFilter(bool enableSearch, bool enableFilters, IEnumerable<FrameworkElement> filters)
        {
            ContentViewModel.ParentAttached += (sender, args) =>
            {
                // Filter
                FilterContainer.EnableSearch = enableSearch;
                FilterContainer.EnableFilters = enableFilters;

                // Add Filters
                Filters = filters;

                // Bind til søgeevent
                FilterContainer.SearchTextChanged += (o, eventArgs) => UpdateFilter(eventArgs.SearchText);

                // Bind checkbox changed
                if (Filters != null)
                    foreach (CheckBox checkbox in Filters.Where(c => c is CheckBox).Cast<CheckBox>())
                    {
                        checkbox.Checked += (s, e) => UpdateFilter();
                        checkbox.Unchecked += (s, e) => UpdateFilter();
                    }
            };
        }

        public void UpdateFilter()
        {
            UpdateFilter("");
        }

        public void UpdateFilter(string searchText)
        {
            TimeCounter.StartTimer();

            IEnumerable<string> checkboxFilters = null;
            if (Filters != null)
                checkboxFilters =
                    Filters.Where(c => c is CheckBox)
                        .Cast<CheckBox>()
                        .Where(c => c.IsChecked.GetValueOrDefault())
                        .Select(c => (string) c.Content)
                        .ToList();

            FilterChanged(this, new FilterEventArgs(searchText, checkboxFilters));

            TimeCounter.StopTime();
        }

        public event EventHandler<FilterEventArgs> FilterChanged;

        public static IEnumerable<T> MergeLists<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var outputList = new List<T>(list1);

            foreach (T elm in list2)
                if (!outputList.Any(e => e.Equals(elm)))
                    outputList.Add(elm);

            return outputList;
        }
    }
}
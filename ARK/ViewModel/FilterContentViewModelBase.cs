using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public abstract class FilterContentViewModelBase : ContentViewModelBase
    {
        public IFilterContainerViewModel FilterContainer
        {
            get { return Parent as IFilterContainerViewModel; }
        }

        protected void EnableFilter(bool search, bool filters)
        {
            ParentAttached += (sender, args) =>
            {
                // Filter
                FilterContainer.EnableSearch = search;
                FilterContainer.EnableFilters = filters;

                // Add Filters
                FilterContainer.Filters = Filters();

                // Bind til søgeevent
                FilterContainer.SearchTextChanged += (o, eventArgs) =>
                {
                    SearchText = eventArgs.SearchText;
                    UpdateFilter();
                };
            };
        }

        protected abstract ObservableCollection<FrameworkElement> Filters(); 
        protected abstract void UpdateFilter();
        
        protected string SearchText { get; set; }

        public IEnumerable<T> MergeLists<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var outputList = new List<T>(list1);

            foreach (T elm in list2)
                if (!outputList.Any(e => e.Equals(elm)))
                    outputList.Add(elm);

            return outputList;
        }
    }
}

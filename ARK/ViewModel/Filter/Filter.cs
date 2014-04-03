using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Filter
{
    class Filter<T> : IFilter<T>
    {
        protected List<IFilterItems<T>> filterItems = new List<IFilterItems<T>>();

        public virtual bool FilterItem(T item)
        {
            foreach (var filter in filterItems)
                if (!filter.FilterItem(item))
                    return false;

            return true;
        }

        #region ICollection
        public IEnumerable<T> FilterItems(T[] items)
        {
            return (from i in items
                    where FilterItem(i)
                    select i);
        }

        public void Add(IFilterItems<T> item)
        {
            filterItems.Add(item);
        }

        public void Clear()
        {
            filterItems.Clear();
        }

        public bool Contains(IFilterItems<T> item)
        {
            return filterItems.Contains(item);
        }

        public void CopyTo(IFilterItems<T>[] array, int arrayIndex)
        {
            filterItems.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return filterItems.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IFilterItems<T> item)
        {
            return filterItems.Remove(item);
        }

        public IEnumerator<IFilterItems<T>> GetEnumerator()
        {
            return filterItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return filterItems.GetEnumerator();
        }
        #endregion
    }
}

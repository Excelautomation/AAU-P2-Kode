using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Filter
{
    public abstract class FilterItems<T> : IFilterItems<T>
    {
        protected Predicate<T> predicate;

        public FilterItems(Predicate<T> predicate) {
            this.predicate = predicate;
        }

        public abstract bool Active();
        public override bool FilterItem(T item)
        {
            if (!Active())
                return true;

            return predicate(item);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    public class CategoryFilter<T> : Filter
    {
        public CategoryFilter(Func<T, bool> filter)
        {
            Filter = filter;
        }

        public Func<T, bool> Filter { get; set; }

        public override IEnumerable<TInput> FilterItems<TInput>(IEnumerable<TInput> items)
        {
            IEnumerable<T> boats = items.Cast<T>().ToList();
            return boats.Where(o => Filter(o)).Cast<TInput>();
        }
    }
}
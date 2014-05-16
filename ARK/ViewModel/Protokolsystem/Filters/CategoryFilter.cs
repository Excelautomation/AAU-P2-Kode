using System;
using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    public class CategoryFilter<T> : IFilter
    {
        #region Constructors and Destructors

        public CategoryFilter(Func<T, bool> filter)
        {
            this.Filter = filter;
        }

        #endregion

        #region Public Properties

        public Func<T, bool> Filter { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<TInput> FilterItems<TInput>(IEnumerable<TInput> items)
        {
            IEnumerable<T> boats = items.Cast<T>().ToList();
            return boats.Where(o => this.Filter(o)).Cast<TInput>();
        }

        #endregion
    }
}
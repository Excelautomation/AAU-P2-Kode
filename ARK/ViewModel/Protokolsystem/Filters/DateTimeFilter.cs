using System;
using System.Collections.Generic;
using System.Linq;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    public class DateTimeFilter : IFilter
    {
        #region Public Properties

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (typeof(T) != typeof(TripViewModel))
            {
                return items;
            }

            IEnumerable<TripViewModel> trips = items.Cast<TripViewModel>().ToList();
            return
                trips.Where(
                    o =>
                        (!this.StartDate.HasValue || o.Trip.TripStartTime.Date >= this.StartDate.Value.Date)
                        && (!this.EndDate.HasValue || o.Trip.TripStartTime.Date <= this.EndDate.Value.Date)).Cast<T>();
        }

        #endregion
    }
}
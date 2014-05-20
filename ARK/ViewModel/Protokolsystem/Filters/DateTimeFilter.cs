using System;
using System.Collections.Generic;
using System.Linq;

using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    public class DateTimeFilter : IFilter
    {
        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

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
                    (!StartDate.HasValue || o.Trip.TripStartTime.Date >= StartDate.Value.Date)
                    && (!EndDate.HasValue || o.Trip.TripStartTime.Date <= EndDate.Value.Date)).Cast<T>();
        }
    }
}
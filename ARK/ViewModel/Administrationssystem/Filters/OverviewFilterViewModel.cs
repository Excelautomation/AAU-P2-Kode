using System;
using System.Collections.Generic;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class OverviewFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public OverViewFilter CurrentFilter { get; set; }

        public OverviewFilterViewModel()
        {
            CurrentFilter = new OverViewFilter();

            Damages = true;
            LongTrip = true;
            BoatsOut = true;
        }

        public bool Damages
        {
            get { return CurrentFilter.ShowDamages; }
            set
            {
                CurrentFilter.ShowDamages = value;
                Notify();
                CallEvent();
            }
        }

        public bool LongTrip
        {
            get { return CurrentFilter.ShowLongTrip; }
            set
            {
                CurrentFilter.ShowLongTrip = value;
                Notify();
                CallEvent();
            }
        }

        public bool BoatsOut
        {
            get { return CurrentFilter.ShowBoatsOut; }
            set
            {
                CurrentFilter.ShowBoatsOut = value;
                Notify();
                CallEvent();
            }
        }

        private void CallEvent()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventArgs(new List<Filter> {CurrentFilter}));
        }

        public event EventHandler<FilterEventArgs> FilterChanged;

        public class OverViewFilter : Filter
        {
            public bool ShowDamages { get; set; }
            public bool ShowLongTrip { get; set; }
            public bool ShowBoatsOut { get; set; }

            public override IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!ShowDamages && !ShowLongTrip && !ShowBoatsOut)
                    return items;

                if (typeof (DamageForm) == typeof (T))
                {
                    if (ShowDamages)
                        return items;
                }
                else if (typeof (LongTripForm) == typeof (T))
                {
                    if (ShowLongTrip)
                        return items;
                }
                else if (typeof (Boat) == typeof (T))
                {
                    if (ShowBoatsOut)
                        return items;
                }

                return new List<T>();
            }
        }
    }
}
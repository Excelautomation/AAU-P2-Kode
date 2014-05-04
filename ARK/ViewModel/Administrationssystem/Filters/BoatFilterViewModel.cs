using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    internal class BoatFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public BoatFilterViewModel()
        {
            CurrentFilter = new BoatFilter();
        }

        public BoatFilter CurrentFilter { get; set; }

        public bool ShowBoatsOut
        {
            get { return CurrentFilter.ShowBoatsOut; }
            set
            {
                CurrentFilter.ShowBoatsOut = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsHome
        {
            get { return CurrentFilter.ShowBoatsHome; }
            set
            {
                CurrentFilter.ShowBoatsHome = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsUnderService
        {
            get { return CurrentFilter.ShowBoatsUnderService; }
            set
            {
                CurrentFilter.ShowBoatsUnderService = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsDamaged
        {
            get { return CurrentFilter.ShowBoatsDamaged; }
            set
            {
                CurrentFilter.ShowBoatsDamaged = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowInactiveBoats
        {
            get { return CurrentFilter.ShowInactiveBoats; }
            set
            {
                CurrentFilter.ShowInactiveBoats = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowFunctionalBoats
        {
            get { return CurrentFilter.ShowFunctionalBoats; }
            set
            {
                CurrentFilter.ShowFunctionalBoats = value;
                Notify();

                CallEvent();
            }
        }

        public event EventHandler<FilterEventArgs> FilterChanged;

        private void CallEvent()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventArgs(new List<Filter> {CurrentFilter}));
        }

        public class BoatFilter : Filter
        {
            public bool ShowBoatsOut { get; set; }
            public bool ShowBoatsHome { get; set; }
            public bool ShowBoatsUnderService { get; set; }
            public bool ShowBoatsDamaged { get; set; }
            public bool ShowInactiveBoats { get; set; }
            public bool ShowFunctionalBoats { get; set; }

            public override IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!ShowBoatsOut && !ShowBoatsHome && !ShowBoatsUnderService && !ShowBoatsDamaged && !ShowInactiveBoats &&
                    !ShowFunctionalBoats)
                    return items;

                if (typeof (Boat) != typeof (T))
                    return items;

                IEnumerable<Boat> boats = items.Cast<Boat>().ToList();
                var output = new List<Boat>();

                if (ShowBoatsOut)
                    output = FilterContent.MergeLists(output, boats.Where(boat => boat.BoatOut)).ToList();
                if (ShowBoatsHome)
                    output = FilterContent.MergeLists(output, boats.Where(boat => !boat.BoatOut)).ToList();
                if (ShowBoatsUnderService)
                    output =
                        FilterContent.MergeLists(output, boats.Where(boat => boat.Damaged && !boat.Usable)).ToList();
                if (ShowBoatsDamaged)
                    output = FilterContent.MergeLists(output, boats.Where(boat => boat.Damaged)).ToList();
                if (ShowInactiveBoats)
                    output = FilterContent.MergeLists(output, boats.Where(boat => !boat.Active)).ToList();
                if (ShowFunctionalBoats)
                    output = FilterContent.MergeLists(output, boats.Where(boat => boat.Usable && boat.Active)).ToList();

                return output.Cast<T>();
            }
        }
    }
}
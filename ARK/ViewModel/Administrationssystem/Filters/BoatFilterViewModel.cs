using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    internal class BoatFilterViewModel : FilterViewModelBase
    {
        public BoatFilterViewModel()
        {
            CurrentFilter = new BoatFilter();
            ShowBoatsOut = true;
            ShowBoatsHome = true;
            ShowBoatsUnderService = true;
            ShowBoatsDamaged = true;
            ShowInactiveBoats = true;
            ShowFunctionalBoats = true;
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

        private void CallEvent()
        {
            OnFilterChanged();
        }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> {CurrentFilter};
        }

        public class BoatFilter : IFilter
        {
            public bool ShowBoatsOut { get; set; }
            public bool ShowBoatsHome { get; set; }
            public bool ShowBoatsUnderService { get; set; }
            public bool ShowBoatsDamaged { get; set; }
            public bool ShowInactiveBoats { get; set; }
            public bool ShowFunctionalBoats { get; set; }

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!ShowBoatsOut && !ShowBoatsHome && !ShowBoatsUnderService && !ShowBoatsDamaged && !ShowInactiveBoats &&
                    !ShowFunctionalBoats)
                    return new List<T>();

                if (typeof (Boat) != typeof (T))
                    return items;

                IEnumerable<Boat> boats = items.Cast<Boat>().ToList();
                var outputBoatsOutIn = new List<Boat>();
                var outputDamage = new List<Boat>();

                if (ShowBoatsOut)
                    outputBoatsOutIn = FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => boat.BoatOut)).ToList();
                if (ShowBoatsHome)
                    outputBoatsOutIn = FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => !boat.BoatOut)).ToList();

                if (ShowBoatsUnderService)
                    outputDamage =
                        FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged && !boat.Usable)).ToList();
                if (ShowBoatsDamaged)
                    outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged)).ToList();
                if (ShowInactiveBoats)
                    outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => !boat.Active)).ToList();
                if (ShowFunctionalBoats)
                    outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Usable && boat.Active)).ToList();

                return outputBoatsOutIn.Where(boat => outputDamage.Any(boat2 => boat == boat2)).Cast<T>();
            }
        }
    }
}
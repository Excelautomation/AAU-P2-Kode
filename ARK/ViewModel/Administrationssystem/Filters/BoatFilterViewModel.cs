using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    internal class BoatFilterViewModel : FilterViewModelBase, IFilter
    {
        private bool _showBoatsDamaged;

        private bool _showBoatsHome;

        private bool _showBoatsOut;

        private bool _showBoatsUnderService;

        private bool _showFunctionalBoats;

        private bool _showInactiveBoats;

        public BoatFilterViewModel()
        {
            ShowBoatsOut = true;
            ShowBoatsHome = true;
            ShowBoatsUnderService = true;
            ShowBoatsDamaged = true;
            ShowInactiveBoats = true;
            ShowFunctionalBoats = true;
        }

        public bool ShowBoatsDamaged
        {
            get
            {
                return _showBoatsDamaged;
            }

            set
            {
                _showBoatsDamaged = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsHome
        {
            get
            {
                return _showBoatsHome;
            }

            set
            {
                _showBoatsHome = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsOut
        {
            get
            {
                return _showBoatsOut;
            }

            set
            {
                _showBoatsOut = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowBoatsUnderService
        {
            get
            {
                return _showBoatsUnderService;
            }

            set
            {
                _showBoatsUnderService = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowFunctionalBoats
        {
            get
            {
                return _showFunctionalBoats;
            }

            set
            {
                _showFunctionalBoats = value;
                Notify();

                CallEvent();
            }
        }

        public bool ShowInactiveBoats
        {
            get
            {
                return _showInactiveBoats;
            }

            set
            {
                _showInactiveBoats = value;
                Notify();

                CallEvent();
            }
        }

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (!ShowBoatsOut && !ShowBoatsHome && !ShowBoatsUnderService && !ShowBoatsDamaged && !ShowInactiveBoats
                && !ShowFunctionalBoats)
            {
                return new List<T>();
            }

            if (typeof(Boat) != typeof(T))
            {
                return items;
            }

            IEnumerable<Boat> boats = items.Cast<Boat>().ToList();
            var outputBoatsOutIn = new List<Boat>();
            var outputDamage = new List<Boat>();

            if (ShowBoatsOut)
            {
                outputBoatsOutIn =
                    FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => boat.BoatOut)).ToList();
            }

            if (ShowBoatsHome)
            {
                outputBoatsOutIn =
                    FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => !boat.BoatOut)).ToList();
            }

            if (ShowBoatsUnderService)
            {
                outputDamage =
                    FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged && !boat.Usable)).ToList();
            }

            if (ShowBoatsDamaged)
            {
                outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged)).ToList();
            }

            if (ShowInactiveBoats)
            {
                outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => !boat.Active)).ToList();
            }

            if (ShowFunctionalBoats)
            {
                outputDamage =
                    FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Usable && boat.Active)).ToList();
            }

            return outputBoatsOutIn.Where(boat => outputDamage.Any(boat2 => boat == boat2)).Cast<T>();
        }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this };
        }

        private void CallEvent()
        {
            OnFilterChanged();
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    internal class BoatFilterViewModel : FilterViewModelBase
    {
        #region Constructors and Destructors

        public BoatFilterViewModel()
        {
            this.CurrentFilter = new BoatFilter();
            this.ShowBoatsOut = true;
            this.ShowBoatsHome = true;
            this.ShowBoatsUnderService = true;
            this.ShowBoatsDamaged = true;
            this.ShowInactiveBoats = true;
            this.ShowFunctionalBoats = true;
        }

        #endregion

        #region Public Properties

        public BoatFilter CurrentFilter { get; set; }

        public bool ShowBoatsDamaged
        {
            get
            {
                return this.CurrentFilter.ShowBoatsDamaged;
            }

            set
            {
                this.CurrentFilter.ShowBoatsDamaged = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsHome
        {
            get
            {
                return this.CurrentFilter.ShowBoatsHome;
            }

            set
            {
                this.CurrentFilter.ShowBoatsHome = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsOut
        {
            get
            {
                return this.CurrentFilter.ShowBoatsOut;
            }

            set
            {
                this.CurrentFilter.ShowBoatsOut = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsUnderService
        {
            get
            {
                return this.CurrentFilter.ShowBoatsUnderService;
            }

            set
            {
                this.CurrentFilter.ShowBoatsUnderService = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowFunctionalBoats
        {
            get
            {
                return this.CurrentFilter.ShowFunctionalBoats;
            }

            set
            {
                this.CurrentFilter.ShowFunctionalBoats = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowInactiveBoats
        {
            get
            {
                return this.CurrentFilter.ShowInactiveBoats;
            }

            set
            {
                this.CurrentFilter.ShowInactiveBoats = value;
                this.Notify();

                this.CallEvent();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this.CurrentFilter };
        }

        #endregion

        #region Methods

        private void CallEvent()
        {
            this.OnFilterChanged();
        }

        #endregion

        public class BoatFilter : IFilter
        {
            #region Public Properties

            public bool ShowBoatsDamaged { get; set; }

            public bool ShowBoatsHome { get; set; }

            public bool ShowBoatsOut { get; set; }

            public bool ShowBoatsUnderService { get; set; }

            public bool ShowFunctionalBoats { get; set; }

            public bool ShowInactiveBoats { get; set; }

            #endregion

            #region Public Methods and Operators

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!this.ShowBoatsOut && !this.ShowBoatsHome && !this.ShowBoatsUnderService && !this.ShowBoatsDamaged
                    && !this.ShowInactiveBoats && !this.ShowFunctionalBoats)
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

                if (this.ShowBoatsOut)
                {
                    outputBoatsOutIn =
                        FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => boat.BoatOut)).ToList();
                }

                if (this.ShowBoatsHome)
                {
                    outputBoatsOutIn =
                        FilterContent.MergeLists(outputBoatsOutIn, boats.Where(boat => !boat.BoatOut)).ToList();
                }

                if (this.ShowBoatsUnderService)
                {
                    outputDamage =
                        FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged && !boat.Usable))
                            .ToList();
                }

                if (this.ShowBoatsDamaged)
                {
                    outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Damaged)).ToList();
                }

                if (this.ShowInactiveBoats)
                {
                    outputDamage = FilterContent.MergeLists(outputDamage, boats.Where(boat => !boat.Active)).ToList();
                }

                if (this.ShowFunctionalBoats)
                {
                    outputDamage =
                        FilterContent.MergeLists(outputDamage, boats.Where(boat => boat.Usable && boat.Active)).ToList();
                }

                return outputBoatsOutIn.Where(boat => outputDamage.Any(boat2 => boat == boat2)).Cast<T>();
            }

            #endregion
        }
    }
}
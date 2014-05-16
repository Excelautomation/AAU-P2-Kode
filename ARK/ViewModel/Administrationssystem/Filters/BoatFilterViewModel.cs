using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    internal class BoatFilterViewModel : FilterViewModelBase, IFilter
    {
        private bool _showInactiveBoats;
        private bool _showBoatsDamaged;
        private bool _showBoatsHome;
        private bool _showBoatsOut;
        private bool _showBoatsUnderService;
        private bool _showFunctionalBoats;

        #region Constructors and Destructors

        public BoatFilterViewModel()
        {
            this.ShowBoatsOut = true;
            this.ShowBoatsHome = true;
            this.ShowBoatsUnderService = true;
            this.ShowBoatsDamaged = true;
            this.ShowInactiveBoats = true;
            this.ShowFunctionalBoats = true;
        }

        #endregion

        #region Public Properties

        public bool ShowBoatsDamaged
        {
            get
            {
                return this._showBoatsDamaged;
            }

            set
            {
                this._showBoatsDamaged = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsHome
        {
            get
            {
                return this._showBoatsHome;
            }

            set
            {
                this._showBoatsHome = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsOut
        {
            get
            {
                return this._showBoatsOut;
            }

            set
            {
                this._showBoatsOut = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowBoatsUnderService
        {
            get
            {
                return this._showBoatsUnderService;
            }

            set
            {
                this._showBoatsUnderService = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowFunctionalBoats
        {
            get
            {
                return this._showFunctionalBoats;
            }

            set
            {
                this._showFunctionalBoats = value;
                this.Notify();

                this.CallEvent();
            }
        }

        public bool ShowInactiveBoats
        {
            get
            {
                return this._showInactiveBoats;
            }

            set
            {
                this._showInactiveBoats = value;
                this.Notify();

                this.CallEvent();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this };
        }

        #endregion

        #region Methods

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

        private void CallEvent()
        {
            this.OnFilterChanged();
        }

        #endregion
    }
}
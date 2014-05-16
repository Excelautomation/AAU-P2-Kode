using System.Collections.Generic;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class OverviewFilterViewModel : FilterViewModelBase
    {
        #region Constructors and Destructors

        public OverviewFilterViewModel()
        {
            this.CurrentFilter = new OverViewFilter();

            this.Damages = true;
            this.LongTrip = true;
            this.BoatsOut = true;
        }

        #endregion

        #region Public Properties

        public bool BoatsOut
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

        public OverViewFilter CurrentFilter { get; set; }

        public bool Damages
        {
            get
            {
                return this.CurrentFilter.ShowDamages;
            }

            set
            {
                this.CurrentFilter.ShowDamages = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool LongTrip
        {
            get
            {
                return this.CurrentFilter.ShowLongTrip;
            }

            set
            {
                this.CurrentFilter.ShowLongTrip = value;
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

        public class OverViewFilter : IFilter
        {
            #region Public Properties

            public bool ShowBoatsOut { get; set; }

            public bool ShowDamages { get; set; }

            public bool ShowLongTrip { get; set; }

            #endregion

            #region Public Methods and Operators

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!this.ShowDamages && !this.ShowLongTrip && !this.ShowBoatsOut)
                {
                    return new List<T>();
                }

                if (typeof(DamageForm) == typeof(T))
                {
                    if (this.ShowDamages)
                    {
                        return items;
                    }
                }
                else if (typeof(LongTripForm) == typeof(T))
                {
                    if (this.ShowLongTrip)
                    {
                        return items;
                    }
                }
                else if (typeof(Boat) == typeof(T))
                {
                    if (this.ShowBoatsOut)
                    {
                        return items;
                    }
                }

                return new List<T>();
            }

            #endregion
        }
    }
}
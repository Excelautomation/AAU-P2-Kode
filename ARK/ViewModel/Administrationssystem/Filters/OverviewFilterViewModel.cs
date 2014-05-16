using System.Collections.Generic;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class OverviewFilterViewModel : FilterViewModelBase, IFilter
    {
        private bool _boatsOut;
        private bool _showDamages;
        private bool _longTrip;

        #region Constructors and Destructors

        public OverviewFilterViewModel()
        {
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
                return this._boatsOut;
            }

            set
            {
                this._boatsOut = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool Damages
        {
            get
            {
                return _showDamages;
            }

            set
            {
                this._showDamages = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool LongTrip
        {
            get
            {
                return _longTrip;
            }

            set
            {
                this._longTrip = value;
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

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (!this.Damages && !this.LongTrip && !this.BoatsOut)
            {
                return new List<T>();
            }

            if (typeof(DamageForm) == typeof(T))
            {
                if (this.Damages)
                {
                    return items;
                }
            }
            else if (typeof(LongTripForm) == typeof(T))
            {
                if (this.LongTrip)
                {
                    return items;
                }
            }
            else if (typeof(Boat) == typeof(T))
            {
                if (this.BoatsOut)
                {
                    return items;
                }
            }

            return new List<T>();
        }

        #endregion

        #region Methods

        private void CallEvent()
        {
            base.OnFilterChanged();
        }

        #endregion
    }
}
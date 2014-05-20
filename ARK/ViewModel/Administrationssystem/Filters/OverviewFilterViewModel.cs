using System.Collections.Generic;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class OverviewFilterViewModel : FilterViewModelBase, IFilter
    {
        private bool _boatsOut;

        private bool _longTrip;

        private bool _showDamages;

        public OverviewFilterViewModel()
        {
            Damages = true;
            LongTrip = true;
            BoatsOut = true;
        }

        public bool BoatsOut
        {
            get
            {
                return _boatsOut;
            }

            set
            {
                _boatsOut = value;
                Notify();
                CallEvent();
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
                _showDamages = value;
                Notify();
                CallEvent();
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
                _longTrip = value;
                Notify();
                CallEvent();
            }
        }

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (!Damages && !LongTrip && !BoatsOut)
            {
                return new List<T>();
            }

            if (typeof(DamageForm) == typeof(T))
            {
                if (Damages)
                {
                    return items;
                }
            }
            else if (typeof(LongTripForm) == typeof(T))
            {
                if (LongTrip)
                {
                    return items;
                }
            }
            else if (typeof(Boat) == typeof(T))
            {
                if (BoatsOut)
                {
                    return items;
                }
            }

            return new List<T>();
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
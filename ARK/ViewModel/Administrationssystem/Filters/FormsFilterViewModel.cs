using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class FormsFilterViewModel : FilterViewModelBase, IFilter
    {
        private bool _showAccepted;

        private bool _showDenied;

        private bool _showOpen;

        public FormsFilterViewModel()
        {
            ShowOpen = true;
        }

        public bool ShowAccepted
        {
            get
            {
                return _showAccepted;
            }

            set
            {
                _showAccepted = value;
                Notify();
                CallEvent();
            }
        }

        public bool ShowDenied
        {
            get
            {
                return _showDenied;
            }

            set
            {
                _showDenied = value;
                Notify();
                CallEvent();
            }
        }

        public bool ShowOpen
        {
            get
            {
                return _showOpen;
            }

            set
            {
                _showOpen = value;
                Notify();
                CallEvent();
            }
        }

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (typeof(DamageForm) == typeof(T))
            {
                var output = new List<DamageForm>();

                if (ShowOpen)
                {
                    output =
                        FilterContent.MergeLists(items.Cast<DamageForm>().Where(form => !form.Closed), output).ToList();
                }

                if (ShowDenied)
                {
                    output =
                        FilterContent.MergeLists(items.Cast<DamageForm>().Where(form => form.Closed), output).ToList();
                }

                return output.Cast<T>();
            }

            if (typeof(LongTripForm) == typeof(T))
            {
                var output = new List<LongTripForm>();

                if (ShowAccepted)
                {
                    output =
                        FilterContent.MergeLists(
                            items.Cast<LongTripForm>().Where(form => form.Status == LongTripForm.BoatStatus.Accepted), 
                            output).ToList();
                }

                if (ShowDenied)
                {
                    output =
                        FilterContent.MergeLists(
                            items.Cast<LongTripForm>().Where(form => form.Status == LongTripForm.BoatStatus.Denied), 
                            output).ToList();
                }

                if (ShowOpen)
                {
                    output =
                        FilterContent.MergeLists(
                            items.Cast<LongTripForm>().Where(form => form.Status == LongTripForm.BoatStatus.Awaiting), 
                            output).ToList();
                }

                return output.Cast<T>();
            }

            return items;
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
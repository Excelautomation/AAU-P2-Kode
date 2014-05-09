using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class FormsFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public FormsFilter CurrentFormsFilter { get; set; }

        public FormsFilterViewModel()
        {
            CurrentFormsFilter = new FormsFilter();

            ShowOpen = true;
        }

        public bool ShowOpen
        {
            get { return CurrentFormsFilter.ShowOpen; }
            set
            {
                CurrentFormsFilter.ShowOpen = value;
                Notify();
                CallEvent();
            }
        }

        public bool ShowDenied
        {
            get { return CurrentFormsFilter.ShowDenied; }
            set
            {
                CurrentFormsFilter.ShowDenied = value;
                Notify();
                CallEvent();
            }
        }
        public bool ShowAccepted
        {
            get { return CurrentFormsFilter.ShowAccepted; }
            set
            {
                CurrentFormsFilter.ShowAccepted = value;
                Notify();
                CallEvent();
            }
        }

        private void CallEvent()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventArgs(new List<Filter> { CurrentFormsFilter }));
        }

        public event EventHandler<FilterEventArgs> FilterChanged;

        public class FormsFilter : Filter
        {
            public bool ShowOpen { get; set; }
            public bool ShowDenied { get; set; }
            public bool ShowAccepted { get; set; }

            public override IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (typeof(DamageForm) == typeof(T))
                {
                    List<DamageForm> output = new List<DamageForm>();

                    if (ShowAccepted)
                        output = FilterContent.MergeLists(
                            items.Cast<DamageForm>()
                                .Where(form => !form.Closed)
                                , output).ToList();
                    if (ShowDenied)
                        output = FilterContent.MergeLists(
                            items.Cast<DamageForm>()
                                .Where(form => form.Closed)
                                , output).ToList();

                    return output.Cast<T>();
                }
                else if (typeof(LongTripForm) == typeof(T))
                {
                    List<LongTripForm> output = new List<LongTripForm>();

                    if (ShowAccepted)
                        output = FilterContent.MergeLists(
                            items.Cast<LongTripForm>()
                                .Where(form => form.Status == LongTripForm.BoatStatus.Accepted)
                                , output).ToList();
                    if (ShowDenied)
                        output = FilterContent.MergeLists(
                            items.Cast<LongTripForm>()
                                .Where(form => form.Status == LongTripForm.BoatStatus.Denied)
                                , output).ToList();
                    if (ShowOpen)
                        output = FilterContent.MergeLists(
                            items.Cast<LongTripForm>()
                                .Where(form => form.Status == LongTripForm.BoatStatus.Awaiting)
                                , output).ToList();

                    return output.Cast<T>();
                }

                return items;
            }
        }
    }
}
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
        }

        public bool ShowLongDistanceForm
        {
            get { return CurrentFormsFilter.ShowLongDistanceForm; }
            set
            {
                CurrentFormsFilter.ShowLongDistanceForm = value;
                Notify();
                CallEvent();
            }
        }

        public bool ShowDamageTypes
        {
            get { return CurrentFormsFilter.ShowDamageTypes; }
            set
            {
                CurrentFormsFilter.ShowDamageTypes = value;
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
            public bool ShowLongDistanceForm { get; set; }
            public bool ShowDamageTypes { get; set; }
            public bool ShowDenied { get; set; }
            public bool ShowAccepted { get; set; }

            public override IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (!ShowLongDistanceForm && !ShowDamageTypes)
                    return items;

                if (typeof(DamageForm) == typeof(T))
                    if (ShowDamageTypes)
                        return items;
                    else
                        return new List<T>();
                else if (typeof (LongDistanceForm) == typeof (T))
                {
                    if (!ShowLongDistanceForm)
                    {
                        if (ShowAccepted || ShowDenied)
                            throw new NotImplementedException();

                        return new List<T>();
                    }

                    if (ShowAccepted && ShowDenied)
                        throw new NotImplementedException();

                    if (ShowAccepted)
                        return items.Cast<LongDistanceForm>().Where(form => form.Approved.GetValueOrDefault()).ToList().Cast<T>();
                    else if (ShowDenied)
                        return items.Cast<LongDistanceForm>().Where(form => !form.Approved.GetValueOrDefault()).ToList().Cast<T>();

                    return items;
                }

                return items;
            }

            public override bool CanFilter<T>(System.Collections.Generic.IEnumerable<T> items)
            {
                return typeof (LongDistanceForm) == typeof (T) ||
                       typeof(DamageForm) == typeof(T);
            }
        }
    }
}
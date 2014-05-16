using System.Collections.Generic;
using System.Linq;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class FormsFilterViewModel : FilterViewModelBase
    {
        #region Constructors and Destructors

        public FormsFilterViewModel()
        {
            this.CurrentFormsFilter = new FormsFilter();

            this.ShowOpen = true;
        }

        #endregion

        #region Public Properties

        public FormsFilter CurrentFormsFilter { get; set; }

        public bool ShowAccepted
        {
            get
            {
                return this.CurrentFormsFilter.ShowAccepted;
            }

            set
            {
                this.CurrentFormsFilter.ShowAccepted = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool ShowDenied
        {
            get
            {
                return this.CurrentFormsFilter.ShowDenied;
            }

            set
            {
                this.CurrentFormsFilter.ShowDenied = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool ShowOpen
        {
            get
            {
                return this.CurrentFormsFilter.ShowOpen;
            }

            set
            {
                this.CurrentFormsFilter.ShowOpen = value;
                this.Notify();
                this.CallEvent();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this.CurrentFormsFilter };
        }

        #endregion

        #region Methods

        private void CallEvent()
        {
            this.OnFilterChanged();
        }

        #endregion

        public class FormsFilter : IFilter
        {
            #region Public Properties

            public bool ShowAccepted { get; set; }

            public bool ShowDenied { get; set; }

            public bool ShowOpen { get; set; }

            #endregion

            #region Public Methods and Operators

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                if (typeof(DamageForm) == typeof(T))
                {
                    var output = new List<DamageForm>();

                    if (this.ShowOpen)
                    {
                        output =
                            FilterContent.MergeLists(items.Cast<DamageForm>().Where(form => !form.Closed), output)
                                .ToList();
                    }

                    if (this.ShowDenied)
                    {
                        output =
                            FilterContent.MergeLists(items.Cast<DamageForm>().Where(form => form.Closed), output)
                                .ToList();
                    }

                    return output.Cast<T>();
                }

                if (typeof(LongTripForm) == typeof(T))
                {
                    var output = new List<LongTripForm>();

                    if (this.ShowAccepted)
                    {
                        output =
                            FilterContent.MergeLists(
                                items.Cast<LongTripForm>()
                                    .Where(form => form.Status == LongTripForm.BoatStatus.Accepted), 
                                output).ToList();
                    }

                    if (this.ShowDenied)
                    {
                        output =
                            FilterContent.MergeLists(
                                items.Cast<LongTripForm>().Where(form => form.Status == LongTripForm.BoatStatus.Denied), 
                                output).ToList();
                    }

                    if (this.ShowOpen)
                    {
                        output =
                            FilterContent.MergeLists(
                                items.Cast<LongTripForm>()
                                    .Where(form => form.Status == LongTripForm.BoatStatus.Awaiting), 
                                output).ToList();
                    }

                    return output.Cast<T>();
                }

                return items;
            }

            #endregion
        }
    }
}
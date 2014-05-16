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

        #region Constructors and Destructors

        public FormsFilterViewModel()
        {
            this.ShowOpen = true;
        }

        #endregion

        #region Public Properties

        public bool ShowAccepted
        {
            get
            {
                return this._showAccepted;
            }

            set
            {
                this._showAccepted = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool ShowDenied
        {
            get
            {
                return this._showDenied;
            }

            set
            {
                this._showDenied = value;
                this.Notify();
                this.CallEvent();
            }
        }

        public bool ShowOpen
        {
            get
            {
                return this._showOpen;
            }

            set
            {
                this._showOpen = value;
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

        #region Methods

        private void CallEvent()
        {
            this.OnFilterChanged();
        }

        #endregion
    }
}
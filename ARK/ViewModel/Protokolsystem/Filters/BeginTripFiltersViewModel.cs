using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class BeginTripFiltersViewModel : ViewModelBase, IFilterViewModel
    {
        private readonly Func<Boat, bool> _categoryAll = boat => true;
        private readonly Func<Boat, bool> _categoryErgometer = boat => boat.SpecificBoatType == Boat.BoatType.Ergometer;
        private readonly Func<Boat, bool> _categoryGig = boat => boat.SpecificBoatType == Boat.BoatType.Gig;
        private readonly Func<Boat, bool> _categoryInrigger = boat => boat.SpecificBoatType == Boat.BoatType.Inrigger;
        private readonly Func<Boat, bool> _categoryKajak = boat => boat.SpecificBoatType == Boat.BoatType.Kajak;
        private readonly Func<Boat, bool> _categoryOutrigger = boat => boat.SpecificBoatType == Boat.BoatType.Outrigger;
        private bool _categoryAllChecked;
        private bool _categoryErgometerChecked;
        private bool _categoryGigChecked;
        private bool _categoryInriggerChecked;
        private bool _categoryKajakChecked;
        private bool _categoryOutriggerChecked;

        public BeginTripFiltersViewModel()
        {
            CurrentBoatFilter = new BoatCategoryFilter(_categoryAll);

            CategoryAllChecked = true;

            UpdateFilter();
        }

        public BoatCategoryFilter CurrentBoatFilter { get; set; }
        public BoatCategoryFilter CurrentBoatType { get; set; }

        public bool CategoryAllChecked
        {
            get { return _categoryAllChecked; }
            set
            {
                _categoryAllChecked = value;
                if (value)
                    UpdateCategory(_categoryAll);

                Notify();
            }
        }

        public bool CategoryKajakChecked
        {
            get { return _categoryKajakChecked; }
            set
            {
                _categoryKajakChecked = value;
                if (value)
                    UpdateCategory(_categoryKajak);

                Notify();
            }
        }

        public bool CategoryOutriggerChecked
        {
            get { return _categoryOutriggerChecked; }
            set
            {
                _categoryOutriggerChecked = value;
                if (value)
                    UpdateCategory(_categoryOutrigger);

                Notify();
            }
        }

        public bool CategoryErgometerChecked
        {
            get { return _categoryErgometerChecked; }
            set
            {
                _categoryErgometerChecked = value;
                if (value)
                    UpdateCategory(_categoryErgometer);

                Notify();
            }
        }

        public bool CategoryInriggerChecked
        {
            get { return _categoryInriggerChecked; }
            set
            {
                _categoryInriggerChecked = value;
                if (value)
                    UpdateCategory(_categoryInrigger);

                Notify();
            }
        }

        public bool CategoryGigChecked
        {
            get { return _categoryGigChecked; }
            set
            {
                _categoryGigChecked = value;
                if (value)
                    UpdateCategory(_categoryGig);

                Notify();
            }
        }

        public event EventHandler<FilterEventArgs> FilterChanged;

        private void UpdateCategory(Func<Boat, bool> filter)
        {
            CurrentBoatFilter.Filter = filter;

            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventArgs(new List<Filter> {CurrentBoatFilter, CurrentBoatType}));
        }

        public class BoatCategoryFilter : Filter
        {
            public BoatCategoryFilter(Func<Boat, bool> filter)
            {
                Filter = filter;
            }

            public Func<Boat, bool> Filter { get; set; }

            public override IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                IEnumerable<Boat> boats = items.Cast<Boat>().ToList();
                return boats.Where(boat => Filter(boat)).Cast<T>();
            }

            public IEnumerable<Boat> GetBoats<T>(IEnumerable<T> items)
            {
                return items.Cast<Boat>();
            }
        }
    }
}
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
        private readonly Func<Boat, bool> _categoryAllTypes = boat => true;
        private readonly Func<Boat, bool> _categoryErgometer = boat => boat.SpecificBoatType == Boat.BoatType.Ergometer;
        private readonly Func<Boat, bool> _categoryGig = boat => boat.SpecificBoatType == Boat.BoatType.Gig;
        private readonly Func<Boat, bool> _categoryInrigger = boat => boat.SpecificBoatType == Boat.BoatType.Inrigger;
        private readonly Func<Boat, bool> _categoryKajak = boat => boat.SpecificBoatType == Boat.BoatType.Kajak;
        private readonly Func<Boat, bool> _categoryOutrigger = boat => boat.SpecificBoatType == Boat.BoatType.Outrigger;
        private bool _categoryAllTypesChecked;
        private bool _categoryErgometerChecked;
        private bool _categoryGigChecked;
        private bool _categoryInriggerChecked;
        private bool _categoryKajakChecked;
        private bool _categoryOutriggerChecked;

        // Boat size filtering
        private readonly Func<Boat, bool> _categoryOne = boat => boat.NumberofSeats == 1;
        private readonly Func<Boat, bool> _categoryTwo = boat => boat.NumberofSeats == 2 || boat.NumberofSeats == 3;
        private readonly Func<Boat, bool> _categoryFour = boat => boat.NumberofSeats == 4 || boat.NumberofSeats == 5;
        private readonly Func<Boat, bool> _categorySix = boat => boat.NumberofSeats == 6;
        private readonly Func<Boat, bool> _categoryEight = boat => boat.NumberofSeats > 8;
        private readonly Func<Boat, bool> _categoryAllSizes = boat => true;
        private readonly Func<Boat, bool> _categoryLongTour = boat => boat.LongTripBoat;
        private bool _categoryOneChecked;
        private bool _categoryTwoChecked;
        private bool _categoryFourChecked;
        private bool _categorySixChecked;
        private bool _categoryEightChecked;
        private bool _categoryAllSizesChecked;
        private bool _categoryLongTourChecked;

        public BeginTripFiltersViewModel()
        {
            CurrentBoatSizeFilter = new BoatCategoryFilter(_categoryAllTypes);

            CategoryAllChecked = true;
            CategoryAllSizesChecked = true;

            UpdateFilter();
        }

        public BoatCategoryFilter CurrentBoatSizeFilter { get; set; }
        public BoatCategoryFilter CurrentBoatType { get; set; }

        public bool CategoryAllChecked
        {
            get { return _categoryAllTypesChecked; }
            set
            {
                _categoryAllTypesChecked = value;
                if (value)
                    UpdateCategory(_categoryAllTypes);

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

        public bool CategoryOneChecked
        {
            get { return _categoryOneChecked ; }
            set
            {
                _categoryOneChecked = value; 
                
                if (value)
                    UpdateCategory(_categoryOne);

                Notify();
            }
        }
        public bool CategoryTwoChecked
        {
            get { return _categoryTwoChecked; }
            set 
            { 
                _categoryTwoChecked = value;
                if (value)
                    UpdateCategory(_categoryTwo);

                Notify();
            }
        }
        public bool CategoryFourChecked
        {
            get { return _categoryFourChecked; }
            set 
            { 
                _categoryFourChecked = value;
                if (value)
                    UpdateCategory(_categoryFour);

                Notify();
            }
        }
        public bool CategorySixChecked
        {
            get { return _categorySixChecked; }
            set 
            { 
                _categorySixChecked = value;
                if (value)
                    UpdateCategory(_categorySix);

                Notify();
            }
        }
        public bool CategoryEightChecked
        {
            get { return _categoryEightChecked; }
            set 
            { 
                _categoryEightChecked = value;
                if (value)
                    UpdateCategory(_categoryEight);

                Notify();
            }
        }
        public bool CategoryAllSizesChecked
        {
            get { return _categoryAllSizesChecked; }
            set 
            {
                _categoryAllSizesChecked = value;
                if (value)
                    UpdateCategory(_categoryAllSizes);

                Notify();
            }
        }
        public bool CategoryLongTourChecked
        {
            get { return _categoryLongTourChecked; }
            set 
            { 
                _categoryLongTourChecked = value;
                if (value)
                    UpdateCategory(_categoryLongTour);

                Notify();
            }
        }


        public event EventHandler<FilterEventArgs> FilterChanged;

        private void UpdateCategory(Func<Boat, bool> filter)
        {
            CurrentBoatSizeFilter.Filter = filter;

            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventArgs(new List<Filter> {CurrentBoatSizeFilter, CurrentBoatType}));
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
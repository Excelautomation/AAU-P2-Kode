using System;
using System.Collections.Generic;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class BeginTripFiltersViewModel : FilterViewModelBase
    {
        private bool _categoryAllSizesChecked;

        private bool _categoryAllTypesChecked;

        private bool _categoryEightChecked;

        private bool _categoryErgometerChecked;

        private bool _categoryFourChecked;

        private bool _categoryGigChecked;

        private bool _categoryInriggerChecked;

        private bool _categoryKajakChecked;

        private bool _categoryLongTourChecked;

        // Boat size filtering
        private bool _categoryOneChecked;

        private bool _categoryOutriggerChecked;

        private bool _categorySixChecked;

        private bool _categoryTwoChecked;

        public BeginTripFiltersViewModel()
        {
            CurrentBoatType = new CategoryFilter<Boat>(boat => true);
            CurrentBoatSizeFilter = new CategoryFilter<Boat>(boat => true);

            CategoryAllChecked = true;
            CategoryAllSizesChecked = true;

            UpdateFilter();
        }

        public bool CategoryAllChecked
        {
            get
            {
                return _categoryAllTypesChecked;
            }

            set
            {
                _categoryAllTypesChecked = value;
                if (value)
                {
                    UpdateCategory(boat => true);
                }

                Notify();
            }
        }

        public bool CategoryAllSizesChecked
        {
            get
            {
                return _categoryAllSizesChecked;
            }

            set
            {
                _categoryAllSizesChecked = value;
                if (value)
                {
                    UpdateSide(boat => true);
                }

                Notify();
            }
        }

        public bool CategoryEightChecked
        {
            get
            {
                return _categoryEightChecked;
            }

            set
            {
                _categoryEightChecked = value;
                if (value)
                {
                    UpdateSide(boat => boat.NumberofSeats > 8);
                }

                Notify();
            }
        }

        public bool CategoryErgometerChecked
        {
            get
            {
                return _categoryErgometerChecked;
            }

            set
            {
                _categoryErgometerChecked = value;
                if (value)
                {
                    UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Ergometer);
                }

                Notify();
            }
        }

        public bool CategoryFourChecked
        {
            get
            {
                return _categoryFourChecked;
            }

            set
            {
                _categoryFourChecked = value;
                if (value)
                {
                    UpdateSide(boat => boat.NumberofSeats == 4 || boat.NumberofSeats == 5);
                }

                Notify();
            }
        }

        public bool CategoryGigChecked
        {
            get
            {
                return _categoryGigChecked;
            }

            set
            {
                _categoryGigChecked = value;
                if (value)
                {
                    UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Gig);
                }

                Notify();
            }
        }

        public bool CategoryInriggerChecked
        {
            get
            {
                return _categoryInriggerChecked;
            }

            set
            {
                _categoryInriggerChecked = value;
                if (value)
                {
                    UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Inrigger);
                }

                Notify();
            }
        }

        public bool CategoryKajakChecked
        {
            get
            {
                return _categoryKajakChecked;
            }

            set
            {
                _categoryKajakChecked = value;
                if (value)
                {
                    UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Kajak);
                }

                Notify();
            }
        }

        public bool CategoryLongTourChecked
        {
            get
            {
                return _categoryLongTourChecked;
            }

            set
            {
                _categoryLongTourChecked = value;
                if (value)
                {
                    UpdateSide(boat => boat.LongTripBoat);
                }

                Notify();
            }
        }

        public bool CategoryOneChecked
        {
            get
            {
                return _categoryOneChecked;
            }

            set
            {
                _categoryOneChecked = value;

                if (value)
                {
                    UpdateSide(boat => boat.NumberofSeats == 1);
                }

                Notify();
            }
        }

        public bool CategoryOutriggerChecked
        {
            get
            {
                return _categoryOutriggerChecked;
            }

            set
            {
                _categoryOutriggerChecked = value;
                if (value)
                {
                    UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Outrigger);
                }

                Notify();
            }
        }

        public bool CategorySixChecked
        {
            get
            {
                return _categorySixChecked;
            }

            set
            {
                _categorySixChecked = value;
                if (value)
                {
                    UpdateSide(boat => boat.NumberofSeats == 6);
                }

                Notify();
            }
        }

        public bool CategoryTwoChecked
        {
            get
            {
                return _categoryTwoChecked;
            }

            set
            {
                _categoryTwoChecked = value;
                if (value)
                {
                    UpdateSide(boat => boat.NumberofSeats == 2 || boat.NumberofSeats == 3);
                }

                Notify();
            }
        }

        public CategoryFilter<Boat> CurrentBoatSizeFilter { get; set; }

        public CategoryFilter<Boat> CurrentBoatType { get; set; }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { CurrentBoatType, CurrentBoatSizeFilter };
        }

        private void UpdateCategory(Func<Boat, bool> filter)
        {
            CurrentBoatType.Filter = filter;

            UpdateFilter();
        }

        private void UpdateFilter()
        {
            OnFilterChanged();
        }

        private void UpdateSide(Func<Boat, bool> filter)
        {
            CurrentBoatSizeFilter.Filter = filter;
            UpdateFilter();
        }
    }
}
using System;
using System.Collections.Generic;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Protokolsystem.Filters
{
    internal class BeginTripFiltersViewModel : FilterViewModelBase
    {
        #region Fields

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

        #endregion

        #region Constructors and Destructors

        public BeginTripFiltersViewModel()
        {
            this.CurrentBoatType = new CategoryFilter<Boat>(boat => true);
            this.CurrentBoatSizeFilter = new CategoryFilter<Boat>(boat => true);

            this.CategoryAllChecked = true;
            this.CategoryAllSizesChecked = true;

            this.UpdateFilter();
        }

        #endregion

        #region Public Properties

        public bool CategoryAllChecked
        {
            get
            {
                return this._categoryAllTypesChecked;
            }

            set
            {
                this._categoryAllTypesChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => true);
                }

                this.Notify();
            }
        }

        public bool CategoryAllSizesChecked
        {
            get
            {
                return this._categoryAllSizesChecked;
            }

            set
            {
                this._categoryAllSizesChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => true);
                }

                this.Notify();
            }
        }

        public bool CategoryEightChecked
        {
            get
            {
                return this._categoryEightChecked;
            }

            set
            {
                this._categoryEightChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => boat.NumberofSeats > 8);
                }

                this.Notify();
            }
        }

        public bool CategoryErgometerChecked
        {
            get
            {
                return this._categoryErgometerChecked;
            }

            set
            {
                this._categoryErgometerChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Ergometer);
                }

                this.Notify();
            }
        }

        public bool CategoryFourChecked
        {
            get
            {
                return this._categoryFourChecked;
            }

            set
            {
                this._categoryFourChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => boat.NumberofSeats == 4 || boat.NumberofSeats == 5);
                }

                this.Notify();
            }
        }

        public bool CategoryGigChecked
        {
            get
            {
                return this._categoryGigChecked;
            }

            set
            {
                this._categoryGigChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Gig);
                }

                this.Notify();
            }
        }

        public bool CategoryInriggerChecked
        {
            get
            {
                return this._categoryInriggerChecked;
            }

            set
            {
                this._categoryInriggerChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Inrigger);
                }

                this.Notify();
            }
        }

        public bool CategoryKajakChecked
        {
            get
            {
                return this._categoryKajakChecked;
            }

            set
            {
                this._categoryKajakChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Kajak);
                }

                this.Notify();
            }
        }

        public bool CategoryLongTourChecked
        {
            get
            {
                return this._categoryLongTourChecked;
            }

            set
            {
                this._categoryLongTourChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => boat.LongTripBoat);
                }

                this.Notify();
            }
        }

        public bool CategoryOneChecked
        {
            get
            {
                return this._categoryOneChecked;
            }

            set
            {
                this._categoryOneChecked = value;

                if (value)
                {
                    this.UpdateSide(boat => boat.NumberofSeats == 1);
                }

                this.Notify();
            }
        }

        public bool CategoryOutriggerChecked
        {
            get
            {
                return this._categoryOutriggerChecked;
            }

            set
            {
                this._categoryOutriggerChecked = value;
                if (value)
                {
                    this.UpdateCategory(boat => boat.SpecificBoatType == Boat.BoatType.Outrigger);
                }

                this.Notify();
            }
        }

        public bool CategorySixChecked
        {
            get
            {
                return this._categorySixChecked;
            }

            set
            {
                this._categorySixChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => boat.NumberofSeats == 6);
                }

                this.Notify();
            }
        }

        public bool CategoryTwoChecked
        {
            get
            {
                return this._categoryTwoChecked;
            }

            set
            {
                this._categoryTwoChecked = value;
                if (value)
                {
                    this.UpdateSide(boat => boat.NumberofSeats == 2 || boat.NumberofSeats == 3);
                }

                this.Notify();
            }
        }

        public CategoryFilter<Boat> CurrentBoatSizeFilter { get; set; }

        public CategoryFilter<Boat> CurrentBoatType { get; set; }

        #endregion

        #region Public Methods and Operators

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this.CurrentBoatType, this.CurrentBoatSizeFilter };
        }

        #endregion

        #region Methods

        private void UpdateCategory(Func<Boat, bool> filter)
        {
            this.CurrentBoatType.Filter = filter;

            this.UpdateFilter();
        }

        private void UpdateFilter()
        {
            this.OnFilterChanged();
        }

        private void UpdateSide(Func<Boat, bool> filter)
        {
            this.CurrentBoatSizeFilter.Filter = filter;
            this.UpdateFilter();
        }

        #endregion
    }
}
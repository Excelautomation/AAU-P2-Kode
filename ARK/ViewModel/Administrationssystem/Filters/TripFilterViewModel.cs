using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    using System.Windows;

    public class TripFilterViewModel : FilterViewModelBase, IFilter
    {
        #region Fields

        private IDictionary<CheckBox, Func<IEnumerable<Trip>, IEnumerable<Trip>>> filters;

        #endregion

        #region Constructors and Destructors

        public TripFilterViewModel()
        {
            // Initialize lists
            this.filters = new Dictionary<CheckBox, Func<IEnumerable<Trip>, IEnumerable<Trip>>>();
            this.ControlsBoatType = new List<CheckBox>();
            this.ControlsDistance = new List<CheckBox>();
            this.ControlsYear = new List<CheckBox>();

            // Add filters - first boattype
            foreach (var boattype in Boat.GetBoatTypes())
            {
                this.AddCheckBox(
                    boattype.ToString(), 
                    (trips) => trips.Where(trip => trip.Boat.SpecificBoatType == boattype), 
                    this.ControlsBoatType);
            }

            // Add distance
            this.AddDistanceCheckBox("Mindre end 5 km", 0, 5, this.ControlsDistance);
            this.AddDistanceCheckBox("5 til 10 km.", 5, 10, this.ControlsDistance);
            this.AddDistanceCheckBox("Mere end 10 km.", 10, int.MaxValue, this.ControlsDistance);

            // Add year
            using (var db = new DbArkContext())
            {
                IEnumerable<int> years = db.Trip.Select(trip => trip.TripStartTime.Year).Distinct();

                foreach (var year in years)
                {
                    this.AddYearCheckbox(year, this.ControlsYear);
                }
            }
        }

        #endregion

        #region Public Properties

        public ICollection<CheckBox> ControlsBoatType { get; private set; }

        public ICollection<CheckBox> ControlsDistance { get; private set; }

        public ICollection<CheckBox> ControlsYear { get; private set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (typeof(Trip) != typeof(T))
            {
                return new List<T>();
            }

            IEnumerable<Trip> trips = items.Cast<Trip>().ToList();

            IEnumerable<Trip> boatTypes = new List<Trip>();
            IEnumerable<Trip> distance = new List<Trip>();
            IEnumerable<Trip> year = new List<Trip>();

            Func<CheckBox, bool> checkBoxSelector = control => control.IsChecked.HasValue && control.IsChecked.Value;

            foreach (var control in this.ControlsBoatType.Where(checkBoxSelector))
            {
                boatTypes = FilterContent.MergeLists(boatTypes, this.filters[control](trips));
            }

            foreach (var control in this.ControlsDistance.Where(checkBoxSelector))
            {
                distance = FilterContent.MergeLists(distance, this.filters[control](trips));
            }

            foreach (var control in this.ControlsYear.Where(checkBoxSelector))
            {
                year = FilterContent.MergeLists(year, this.filters[control](trips));
            }

            return
                boatTypes.Where(trip => distance.Any(trip2 => trip == trip2) && year.Any(trip2 => trip == trip2))
                    .OrderByDescending(trip => trip.TripStartTime)
                    .Cast<T>();
        }

        public override IEnumerable<IFilter> GetFilter()
        {
            return new List<IFilter> { this };
        }

        #endregion

        #region Methods

        private void AddCheckBox(
            string text, 
            Func<IEnumerable<Trip>, IEnumerable<Trip>> func, 
            ICollection<CheckBox> targetList)
        {
            // Add checkbox and bind events
            CheckBox checkBox = new CheckBox { Content = text, IsChecked = true };
            checkBox.Checked += this.checkBox_CheckedChanged;
            checkBox.Unchecked += this.checkBox_CheckedChanged;

            // Add to list
            targetList.Add(checkBox);

            // Add to dictionary
            this.filters.Add(checkBox, func);
        }

        private void AddDistanceCheckBox(string text, int from, int to, ICollection<CheckBox> targetList)
        {
            this.AddCheckBox(
                text, 
                (trips) => trips.Where(trip => trip.Distance >= from && trip.Distance <= to), 
                targetList);
        }

        private void AddYearCheckbox(int year, ICollection<CheckBox> targetList)
        {
            this.AddCheckBox(
                year.ToString(), 
                (trips) => trips.Where(trip => trip.TripStartTime.Year == year), 
                targetList);
        }

        private void checkBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.OnFilterChanged();
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.View.Administrationssystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class TripsViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        #region Fields

        public BoatListWindow NewBoatDialog;

        private List<Boat> _allBoats;

        private Trip _currentTrip;

        private bool _recentSave;

        private List<Trip> _trips;

        private IEnumerable<Trip> _tripsFiltered;

        #endregion

        #region Constructors and Destructors

        public TripsViewModel()
        {
            this.ParentAttached += (sender, e) =>
                {
                    // Load data
                    using (var db = new DbArkContext())
                    {
                        this._trips = db.Trip.Include(trip => trip.Members).ToList();

                        this._allBoats = db.Boat.ToList();

                        if (this._trips.Count != 0)
                        {
                            this.CurrentTrip = this._trips[0];

                            // LocalTrip = CurrentTrip;
                        }
                    }

                    // Reset filter
                    this.ResetFilter();
                };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => this.UpdateFilter(eventArgs);
        }

        #endregion

        #region Public Properties

        public List<Boat> AllBoats
        {
            get
            {
                return this._allBoats;
            }

            set
            {
                this._allBoats = value;
                this.Notify();
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                return this.GetCommand(this.Reload);
            }
        }

        public Trip CurrentTrip
        {
            get
            {
                return this._currentTrip;
            }

            set
            {
                this._currentTrip = value;
                this.Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return new TripFilter();
            }
        }

        public bool RecentSave
        {
            get
            {
                return this._recentSave;
            }

            set
            {
                this._recentSave = value;
                this.Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                // CurrentTrip = LocalTrip;
                return this.GetCommand(this.Save);
            }
        }

        public ICommand SelectedChange
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            this.CurrentTrip = (Trip)e;

                            // LocalTrip = CurrentTrip;
                            this.RecentSave = false;
                        });
            }
        }

        public ICommand ShowBoatDialog
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.NewBoatDialog = new BoatListWindow();
                            this.NewBoatDialog.DataContext = this;
                            this.NewBoatDialog.ShowDialog();
                        });
            }
        }

        public ICommand ShowBoatsContinue
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            this.NewBoatDialog.Close();

                            this.CurrentTrip.Boat = ((Boat)e);
                            this.CurrentTrip.BoatId = this.CurrentTrip.Boat.Id;

                            this.NotifyCustom("CurrentTrip");
                        });
            }
        }

        public IEnumerable<Trip> TripsFiltered
        {
            get
            {
                return this._tripsFiltered;
            }

            set
            {
                this._tripsFiltered = value;
                this.Notify();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void UpdateList<T>(IEnumerable<T> list)
        {
            var tmp = this.TripsFiltered;
            this.TripsFiltered = null;
            this.TripsFiltered = tmp;
        }

        #endregion

        #region Methods

        private void Reload()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(this.CurrentTrip).State = EntityState.Modified;
                db.Entry(this.CurrentTrip).Reload();
            }

            this.RecentSave = false;

            // Trigger notify - reset lists
            this.UpdateList<Trip>(this.TripsFiltered);

            this.NotifyCustom("CurrentTrip");
        }

        private void ResetFilter()
        {
            this.TripsFiltered = this._trips;
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                Trip trip = db.Trip.Find(this.CurrentTrip.Id);
                db.Entry(trip).CurrentValues.SetValues(this.CurrentTrip);

                db.Entry(trip).State = EntityState.Modified;
                db.Entry(trip.Boat).State = EntityState.Modified;

                db.SaveChanges();
            }

            // Trigger notify - reset lists
            this.UpdateList<Trip>(this.TripsFiltered);

            this.RecentSave = true;
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            this.ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.TripsFiltered = FilterContent.FilterItems(this.TripsFiltered, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.TripsFiltered =
                    this.TripsFiltered.Where(trip => trip.Filter(args.SearchEventArgs.SearchText)).ToList();
            }
        }

        #endregion
    }
}
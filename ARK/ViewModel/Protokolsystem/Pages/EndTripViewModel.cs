using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Confirmations;
using ARK.ViewModel.Base;
using ARK.ViewModel.Protokolsystem.Confirmations;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class EndTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        #region Fields

        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private List<Trip> _activeTrips;

        private double _customDistance;

        private DateTime _latestData;

        private StandardTrip _selectedStdTrip;

        private Trip _selectedTrip;

        private List<StandardTrip> _standardTrips;

        #endregion

        // Constructor
        #region Constructors and Destructors

        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            this.ParentAttached += (sender, args) =>
                {
                    if (this.StandardTrips == null || (DateTime.Now - this._latestData).TotalHours > 1)
                    {
                        // Indlæs data
                        this.GetStandardTrips();

                        this._latestData = DateTime.Now;
                    }

                    this.GetActiveTrips();

                    // Reset selected trip
                    this.SelectedTrip = null;

                    // Bind keyboard
                    // base.ProtocolSystem.KeyboardTextChanged += this.MonitorCustomDistance;
                    this.ResetPage();
                };

            this.ParentDetached += (sender, args) =>
                {
                    // base.ProtocolSystem.KeyboardTextChanged -= this.MonitorCustomDistance;
                };

            TimeCounter.StopTime();
        }

        #endregion

        // Props
        #region Public Properties

        public List<Trip> ActiveTrips
        {
            get
            {
                return this._activeTrips;
            }

            set
            {
                this._activeTrips = value;
                this.Notify();
            }
        }

        public double CustomDistance
        {
            get
            {
                return this._customDistance;
            }

            set
            {
                this._customDistance = value;
                this.Notify();
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return new RelayCommand(
                    e =>
                        {
                            if (this.SelectedStdTrip != null)
                            {
                                this.SelectedTrip.Title = this.SelectedStdTrip.Title;
                                this.SelectedTrip.Direction = this.SelectedStdTrip.Direction;
                                this.SelectedTrip.Distance = this.SelectedStdTrip.Distance;
                            }

                            // set Custom distance if different from default
                            this.SelectedTrip.Distance = this.CustomDistance > 0
                                                             ? this.CustomDistance
                                                             : this.SelectedTrip.Distance;
                            this.SelectedTrip.TripEndedTime = DateTime.Now;
                            this._db.SaveChanges();

                            this.GetActiveTrips();
                            this.ProtocolSystem.UpdateDailyKilometers();
                            this.ProtocolSystem.UpdateNumBoatsOut();
                            this.ResetPage();
                        }, 
                    e => this.SelectedStdTrip != null || this.CustomDistance > 0);
            }
        }

        public StandardTrip SelectedStdTrip
        {
            get
            {
                return this._selectedStdTrip;
            }

            set
            {
                this._selectedStdTrip = value;
                this.Notify();
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return this._selectedTrip;
            }

            set
            {
                this._selectedTrip = value;
                this.Notify();
            }
        }

        public ICommand SetCustomDistance
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            var confirmView = new ChangeDistanceConfirm();
                            var confirmViewModel = (ChangeDistanceConfirmViewModel)confirmView.DataContext;

                            confirmViewModel.SelectedTrip = this.SelectedTrip;
                            this.ProtocolSystem.ShowDialog(confirmView);

                            base.ProtocolSystem.EnableSearch = true;
                        }, 
                    x => this.SelectedTrip != null);
            }
        }

        public List<StandardTrip> StandardTrips
        {
            get
            {
                return this._standardTrips;
            }

            set
            {
                this._standardTrips = value;
                this.Notify();
            }
        }

        public ICommand StdTripSelected
        {
            get
            {
                return this.GetCommand(
                    st =>
                        {
                            var temp = ((IList)st).Cast<StandardTrip>();
                            this.SelectedStdTrip = temp.FirstOrDefault();
                        });
            }
        }

        #endregion

        #region Methods

        private void GetActiveTrips()
        {
            this.ActiveTrips = this._db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }

        private void GetStandardTrips()
        {
            this.StandardTrips = this._db.StandardTrip.OrderBy(trip => trip.Distance).ToList();
        }

        private void ResetPage()
        {
            this.SelectedTrip = null;
            this.SelectedStdTrip = null;
            base.ProtocolSystem.KeyboardClear();
        }

        #endregion
    }
}
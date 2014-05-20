using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

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
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private List<Trip> _activeTrips;

        private double _customDistance;

        private DateTime _latestData;

        private StandardTrip _selectedStdTrip;

        private Trip _selectedTrip;

        private List<StandardTrip> _standardTrips;

        // Constructor
        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            ParentAttached += (sender, args) =>
                {
                    if (StandardTrips == null || (DateTime.Now - _latestData).TotalHours > 1)
                    {
                        // Indlæs data
                        GetStandardTrips();

                        _latestData = DateTime.Now;
                    }

                    GetActiveTrips();

                    // Reset selected trip
                    SelectedTrip = null;

                    // Bind keyboard
                    // base.ProtocolSystem.KeyboardTextChanged += this.MonitorCustomDistance;
                    ResetPage();
                };

            ParentDetached += (sender, args) =>
                {
                    // base.ProtocolSystem.KeyboardTextChanged -= this.MonitorCustomDistance;
                };

            TimeCounter.StopTime();
        }

        // Props
        public List<Trip> ActiveTrips
        {
            get
            {
                return _activeTrips;
            }

            set
            {
                _activeTrips = value;
                Notify();
            }
        }

        public double CustomDistance
        {
            get
            {
                return _customDistance;
            }

            set
            {
                _customDistance = value;
                Notify();
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return new RelayCommand(
                    e =>
                        {
                            if (SelectedStdTrip != null)
                            {
                                SelectedTrip.Title = SelectedStdTrip.Title;
                                SelectedTrip.Direction = SelectedStdTrip.Direction;
                                SelectedTrip.Distance = SelectedStdTrip.Distance;
                            }

                            // set Custom distance if different from default
                            SelectedTrip.Distance = CustomDistance > 0 ? CustomDistance : SelectedTrip.Distance;
                            SelectedTrip.TripEndedTime = DateTime.Now;
                            _db.SaveChanges();

                            GetActiveTrips();
                            ProtocolSystem.UpdateDailyKilometers();
                            ProtocolSystem.UpdateNumBoatsOut();
                            ResetPage();
                        }, 
                    e => SelectedStdTrip != null || CustomDistance > 0);
            }
        }

        public StandardTrip SelectedStdTrip
        {
            get
            {
                return _selectedStdTrip;
            }

            set
            {
                _selectedStdTrip = value;
                Notify();
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;
                Notify();
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

                            confirmViewModel.SelectedTrip = SelectedTrip;
                            ProtocolSystem.ShowDialog(confirmView);

                            base.ProtocolSystem.EnableSearch = true;
                        }, 
                    x => SelectedTrip != null);
            }
        }

        public List<StandardTrip> StandardTrips
        {
            get
            {
                return _standardTrips;
            }

            set
            {
                _standardTrips = value;
                Notify();
            }
        }

        public ICommand StdTripSelected
        {
            get
            {
                return GetCommand(
                    st =>
                        {
                            var temp = ((IList)st).Cast<StandardTrip>();
                            SelectedStdTrip = temp.FirstOrDefault();
                        });
            }
        }

        private void GetActiveTrips()
        {
            ActiveTrips = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }

        private void GetStandardTrips()
        {
            StandardTrips = _db.StandardTrip.OrderBy(trip => trip.Distance).ToList();
        }

        private void ResetPage()
        {
            SelectedTrip = null;
            SelectedStdTrip = null;
            base.ProtocolSystem.KeyboardClear();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.HelperFunctions;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class EndTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<StandardTrip> _standardTrips;
        private List<Trip> _activeTrips;
        private readonly DbArkContext _db = DbArkContext.GetDbContext();
        private Trip _selectedTrip;
        private double _customDistance;
        private readonly Regex _validDistance = new Regex(@"(?'number'\d+(?:(?:,|.)\d+)?)");
        private DateTime _latestData;

        // Constructor
        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            ParentAttached += (sender, args) =>
            {
                if (this.StandardTrips == null || (DateTime.Now - _latestData).TotalHours > 1)
                {
                    // Indlæs data
                    this.GetStandardTrips();

                    _latestData = DateTime.Now;
                }
                this.GetActiveTrips();

                // Reset selected trip
                SelectedTrip = null;

                // Bind keyboard
                base.ProtocolSystem.KeyboardTextChanged += this.MonitorCustomDistance;

                this.ResetPage();
            };

            ParentDetached += (sender, args) =>
            {
                base.ProtocolSystem.KeyboardTextChanged -= this.MonitorCustomDistance;
            };

            TimeCounter.StopTime();
        }

        // Props
        public List<StandardTrip> StandardTrips
        {
            get { return _standardTrips; }
            set
            {
                _standardTrips = value;
                Notify();
            }
        }

        public double CustomDistance
        {
            get { return _customDistance; }
            set { _customDistance = value; Notify(); }
        }

        public List<Trip> ActiveTrips
        {
            get { return _activeTrips; }
            set
            {
                _activeTrips = value;
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

                        var mainViewModel = base.Parent as ProtocolSystemMainViewModel;
                        this.GetActiveTrips();
                        mainViewModel.UpdateDailyKilometers();
                        mainViewModel.UpdateNumBoatsOut();
                    },
                    e => this.SelectedStdTrip != null || this.CustomDistance > 0);
            }
        }

        public ICommand SetCustomDistance
        {
            get
            {
                return new RelayCommand(
                    x => base.ToggleKeyboard.Execute(null),
                    x => this.SelectedTrip != null);
            }
        }

        public ICommand StdTripSelected
        {
            get
            {
                return GetCommand<IList>(st =>
                {
                    var temp = st.Cast<StandardTrip>();
                    this.SelectedStdTrip = temp.FirstOrDefault();
                });
            }
        }

        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set
            {
                _selectedTrip = value;
                Notify();
            }
        }

        private StandardTrip SelectedStdTrip { get; set; }

        private void GetActiveTrips()
        {
            ActiveTrips = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }

        private void GetStandardTrips()
        {
            StandardTrips = _db.StandardTrip.OrderBy(trip => trip.Distance).ToList();
        }

        private void MonitorCustomDistance(object sender, KeyboardEventArgs args)
        {
            CaptureCollection temp;
            this.CustomDistance = (temp = _validDistance.Match(args.Text).Groups["number"].Captures).Count > 0 ? Convert.ToDouble(temp[0].Value) : 0;
        }

        private void ResetPage()
        {
            this.SelectedTrip = null;
            this.SelectedStdTrip = null;
            base.ProtocolSystem.KeyboardClear();
        }
    }
}
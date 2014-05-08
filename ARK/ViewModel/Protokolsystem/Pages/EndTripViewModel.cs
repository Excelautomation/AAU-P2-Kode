using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Media.TextFormatting;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace ARK.ViewModel.Protokolsystem
{
    class EndTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<StandardTrip> _standardTrips;
        private List<Trip> _activeTrips;
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private double _customDistance;
        private bool _canEndTrip;
        private Regex _validDistance = new Regex(@"(?'number'\d+(?:(?:,|.)\d+)?)", RegexOptions.Compiled);
        private DateTime _latestData;

        // Constructor
        public EndTripViewModel()
        {
            TimeCounter.StartTimer();


            ParentAttached += (sender, args) =>
            {
                if (this.ActiveTrips == null || (DateTime.Now - _latestData).TotalHours > 1)
                {
                    // Indlæs data
                    this.GetStandardTrips();

                    _latestData = DateTime.Now;
                }
                this.GetActiveTrips();

                base.ProtocolSystem.KeyboardTextChanged += this.CheckCanEndTrip;
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

        public bool CanEndTrip
        {
            get { return _canEndTrip; }
            set
            {
                _canEndTrip = value;
                Notify();
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return GetCommand<object>(e =>
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
                });
            }
        }

        public ICommand StdTripSelected
        {
            get
            {
                return GetCommand<IList>(st =>
                {
                    var temp = st.Cast<StandardTrip>();
                    if (this.SelectedStdTrips.Count() == 1)
                    {
                        this.CanEndTrip = true;
                        this.SelectedStdTrip = temp.First();
        }
                    else
                {
                        this.CanEndTrip = false;
                        this.SelectedStdTrip = null;
                    }
                });
            }
        }

        public IEnumerable<StandardTrip> SelectedStdTrips { get; set; }

        public Trip SelectedTrip { get; set; }

        private StandardTrip SelectedStdTrip { get; set; }

        private void GetActiveTrips()
        {
            ActiveTrips = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }

        private void GetStandardTrips()
        {
            StandardTrips = _db.StandardTrip.OrderBy(trip => trip.Distance).ToList();
        }

        private void CheckCanEndTrip(object sender, KeyboardEventArgs args)
        {
            CaptureCollection temp;
            if ((temp = _validDistance.Match(args.Text).Groups["number"].Captures).Count > 0)
            {
                this.CustomDistance = Convert.ToDouble(temp[0].Value);
                this.CanEndTrip = true;
            }
            else if (this.SelectedStdTrip == null)
            {
                this.CanEndTrip = false;
            }
        }
    }
}
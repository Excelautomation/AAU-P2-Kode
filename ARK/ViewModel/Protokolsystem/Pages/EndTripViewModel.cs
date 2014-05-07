using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    class EndTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<StandardTrip> _standardTrips = new List<StandardTrip>();
        private List<Trip> _activeTrips = new List<Trip>();
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private double _customDistance;

        // Constructor
        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            GetData();

            ParentAttached += (sender, args) =>
            {
                // Bind på keyboard toggle changed
                Keyboard.PropertyChanged += (senderKeyboard, keyboardArgs) =>
                {
                    // Tjek om toggled er ændret
                    if (keyboardArgs.PropertyName == "KeyboardToggled")
                        NotifyCustom("KeyboardToggleText");
                };

                // Notify at parent er ændret
                NotifyCustom("Keyboard");
                NotifyCustom("KeyboardToggleText");
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
                return GetCommand<object>(e =>
                {
                    if (SelectedStdTrip != null)
                    {
                        SelectedTrip.Title = SelectedStdTrip.Title;
                        SelectedTrip.Direction = SelectedStdTrip.Direction;
                        SelectedTrip.Distance = SelectedStdTrip.Distance;
                    }
                    // set Custom distance if different from default
                    //SelectedTrip.Distance = CustomDistance > 0 ? CustomDistance : 0 ;
                    SelectedTrip.TripEndedTime = DateTime.Now;
                    _db.SaveChanges();

                    var mainViewModel = Parent as ProtocolSystemMainViewModel;
                    mainViewModel.UpdateDailyKilometers();
                    mainViewModel.UpdateNumBoatsOut();
                });
            }
        }

        public ICommand TripSelected
        {
            get
            {
                return GetCommand<Trip>(t =>
                {
                    SelectedTrip = t;
                });
            }
        }

        public ICommand StdTripSelected
        {
            get
            {
                return GetCommand<StandardTrip>(st =>
                {
                    SelectedStdTrip = st;
                });
            }
        }

        private Trip SelectedTrip { get; set; }

        private StandardTrip SelectedStdTrip { get; set; }

        private void GetData()
        {
            // Indlæs data
            StandardTrips = _db.StandardTrip.OrderBy(trip => trip.Distance).ToList();

            ActiveTrips = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }
    }
}
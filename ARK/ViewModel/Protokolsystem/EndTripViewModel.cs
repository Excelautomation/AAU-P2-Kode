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
    class EndTripViewModel : KeyboardContentViewModelBase
    {
        private List<StandardTrip> _standardTrips = new List<StandardTrip>();
        private List<Boat> _boatsOut = new List<Boat>();

        private double _customDistance;

        public double CustomDistance
        {
            get { return _customDistance; }
            set { _customDistance = value; Notify(); }
        }

        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            // Indlæs data
            DbArkContext db = DbArkContext.GetDbContext();
            
                StandardTrips = db.StandardTrip.OrderBy(trip => trip.Distance).ToList();

                BoatsOut = db.Boat.ToList().Where(boat => boat.BoatOut)
                    .OrderByDescending(boat => boat.GetActiveTrip.TripStartTime).ToList();
            

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

        public List<StandardTrip> StandardTrips
        {
            get { return _standardTrips; }
            set
            {
                _standardTrips = value;
                Notify();
            }
        }

        public StandardTrip StdTrip { get; set; }

        public List<Boat> BoatsOut
        {
            get { return _boatsOut; }
            set
            {
                _boatsOut = value;
                Notify();
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return GetCommand<Object>(e =>
                {
                    Trip trip = new Trip();

                    if (StandardTrips != null)
                    {
                        trip.Distance = StdTrip.Distance;
                        trip.Description = StdTrip.Description;
                        trip.Direction = StdTrip.Direction;   
                    }
                                        // set Custom distance if different from default
                    if (CustomDistance > 0)
                        trip.Distance = CustomDistance;
                    //
                    DbArkContext db = DbArkContext.GetDbContext();
                    db.Trip.Add(trip);
                    //
                });
            }
        } 
    }
}
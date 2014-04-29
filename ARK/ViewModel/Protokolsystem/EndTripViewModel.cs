using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Protokolsystem
{
    class EndTripViewModel : KeyboardContentViewModelBase
    {
        private List<StandardTrip> _standardTrips = new List<StandardTrip>();
        private List<Boat> _boatsOut = new List<Boat>();

        public EndTripViewModel()
        {
            TimeCounter.StartTimer();

            // Indlæs data
            using (DbArkContext db = new DbArkContext())
            {
                StandardTrip = db.StandardTrip.OrderBy(trip => trip.Distance).ToList();

                BoatsOut = db.Boat.Where(boat => boat.BoatOut == false)
                    .OrderByDescending(boat => boat.Trips.FirstOrDefault(trip => trip.TripEndedTime == null).TripStartTime).ToList();
            }

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

        public List<StandardTrip> StandardTrip
        {
            get { return _standardTrips; }
            set
            {
                _standardTrips = value;
                Notify();
            }
        }

        public List<Boat> BoatsOut
        {
            get { return _boatsOut; }
            set
            {
                _boatsOut = value;
                Notify();
            }
        }
    }
}
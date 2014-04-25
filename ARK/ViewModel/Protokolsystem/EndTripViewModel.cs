using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Text;
using System.Threading.Tasks;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    class EndTripViewModel : KeyboardContentViewModelBase 
    {
        private List<Boat> _boatsOut = new List<Boat>();

        public EndTripViewModel()
        {
            // Indlæs data
            using (DbArkContext db = new DbArkContext())
            {
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

        public string KeyboardToggleText
        {
            get { return Keyboard.KeyboardToggled ? "SKJUL\nTASTATUR" : "VIS\nTASTATUR"; }
        }

    }
}

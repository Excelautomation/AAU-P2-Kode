using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Data.Entity;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    internal class BoatsOutViewModel : KeyboardContentViewModelBase
    {
        private List<Boat> _boatsOut = new List<Boat>();

        private Boat _selectedBoat;

        public BoatsOutViewModel()
        {
            // Load data
            var db = DbArkContext.GetDbContext();

            BoatsOut = db.Boat.AsEnumerable().Where(boat => boat.BoatOut)
                .OrderByDescending(boat => 
                    boat.Trips.FirstOrDefault(trip => trip.TripEndedTime == null).TripStartTime).ToList();
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

        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
                Notify();
            }
        }

        // Methods
    }
}
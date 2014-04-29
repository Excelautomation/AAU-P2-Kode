using System;
using ARK.Model;
using ARK.Model.DB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel
{
    class BoatsViewModelBase : KeyboardContentViewModelBase
    {
        private List<Boat> _boatsOut = new List<Boat>();

        public BoatsViewModelBase()
        {
            using (DbArkContext db = new DbArkContext())
            {
                BoatsOut = db.Boat.Where(boat => boat.BoatOut == false)
                    .OrderByDescending(boat => boat.Trips.FirstOrDefault(trip => trip.TripEndedTime == null).TripStartTime).ToList();
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

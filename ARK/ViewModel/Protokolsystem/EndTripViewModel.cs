using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Protokolsystem
{
    class EndTripViewModel : Base.ViewModel
    {
        private List<Boat> _boatsOut = new List<Boat>();

        public EndTripViewModel()
        {
            // Indlæs data
            using (DbArkContext db = new DbArkContext())
            {
                BoatsOut = new List<Boat>(db.Boat).Where(boat => boat.BoatOut == true).ToList(); // && boat.TripEnded == false
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

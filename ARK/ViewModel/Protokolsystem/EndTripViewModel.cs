using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel
{
    class EndTripViewModel : Base.ViewModel
    {
        private List<Boat> _boats = new List<Boat>();
        private bool _boatsOut;

        public EndTripViewModel()
        {
            // Indlæs data
            using (DbArkContext db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat);
            }
        }

        public bool BoatsOut
        {
            get
            {
                return Boats.Any(boat => boat.BoatOut);
            }
        }

        public List<Boat> Boats
        {
            get { return _boats; }
            set
            {
                _boats = value;
                Notify();
            }
        }
    }
}

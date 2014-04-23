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
        private bool _boatsOut;

        public EndTripViewModel()
        {
            // Indlæs data
            using (DbArkContext db = new DbArkContext())
            {
                List<Boat> Boats = new List<Boat>(db.Boat);
            }
        }

        public bool BoatsOut
        {
            get
            {
                return _boatsOut;
            }
            set
            {
                _boatsOut = value;
                Notify();
            }
        }
    }
}

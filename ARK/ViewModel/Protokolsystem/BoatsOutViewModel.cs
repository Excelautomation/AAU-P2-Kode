using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Data.Entity;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    // Hvis personen der lavet GUI mangler noget i denne fil henvend dig til Mads Gadeberg hvis du vil have det lavet!
    internal class BoatsOutViewModel : ProtokolsystemContentViewModelBase
    {
        private List<Boat> _boatsOut = new List<Boat>();
        private Boat _selectedBoat;
        private readonly DbArkContext _db = DbArkContext.GetDbContext();


        public BoatsOutViewModel()
        {
            BoatsOut = _db.Trip.Where(t => t.TripEndedTime == null).Select(t => t.Boat).ToList();
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
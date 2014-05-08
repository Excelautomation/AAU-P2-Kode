using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class BoatsOutViewModel : ProtokolsystemContentViewModelBase
    {
        private List<Trip> _boatsOut = new List<Trip>();
        private Boat _selectedBoat;
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        public BoatsOutViewModel()
        {
            ParentAttached += (sender, e) =>
                BoatsOut = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
        }

        public List<Trip> BoatsOut
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
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using System.Collections.ObjectModel;
using ARK.ViewModel.Base.Interfaces;
using ARK.View.Protokolsystem.Additional;
using System.Windows;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class BoatsOutViewModel : ProtokolsystemContentViewModelBase
    {
        private List<Trip> _tripsOngoing = new List<Trip>();
        private Trip _selectedTrip;
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        public BoatsOutViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                TripsOngoing = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
                UpdateInfo();
            };
        }

        public List<Trip> TripsOngoing
        {
            get { return _tripsOngoing; }
            set
            {
                _tripsOngoing = value;
                Notify();
            }
        }

        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set { _selectedTrip = value; Notify(); UpdateInfo(); }
        }

        // Methods
        private void UpdateInfo()
        {
            Info.SelectedTrip = SelectedTrip;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        private IInfoContainerViewModel GetInfoContainerViewModel
        {
            get { return Parent as IInfoContainerViewModel; }
        }
        private FrameworkElement _infoPage;

        public System.Windows.FrameworkElement InfoPage { get { return _infoPage ?? (_infoPage = new BoatsOutAdditionalInfo()); } }

        public BoatsOutAdditionalInfoViewModel Info { get { return InfoPage.DataContext as BoatsOutAdditionalInfoViewModel; } }
    }
}
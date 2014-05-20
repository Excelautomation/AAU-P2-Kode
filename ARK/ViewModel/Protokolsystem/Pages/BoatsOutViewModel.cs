using System.Collections.Generic;
using System.Linq;
using System.Windows;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class BoatsOutViewModel : ProtokolsystemContentViewModelBase
    {
        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private FrameworkElement _infoPage;

        private Trip _selectedTrip;

        private List<Trip> _tripsOngoing = new List<Trip>();

        public BoatsOutViewModel()
        {
            ParentAttached += (sender, e) =>
                {
                    TripsOngoing = _db.Trip.Where(t => t.TripEndedTime == null).ToList();
                    UpdateInfo();
                };
        }

        public BoatsOutAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as BoatsOutAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return _infoPage ?? (_infoPage = new BoatsOutAdditionalInfo());
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;
                Notify();
                UpdateInfo();
            }
        }

        public List<Trip> TripsOngoing
        {
            get
            {
                return _tripsOngoing;
            }

            set
            {
                _tripsOngoing = value;
                Notify();
            }
        }

        // Methods
        private IInfoContainerViewModel GetInfoContainerViewModel
        {
            get
            {
                return Parent as IInfoContainerViewModel;
            }
        }

        private void UpdateInfo()
        {
            Info.SelectedTrip = SelectedTrip;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }
    }
}
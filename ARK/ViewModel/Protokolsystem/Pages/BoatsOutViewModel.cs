using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #region Fields

        private readonly DbArkContext _db = DbArkContext.GetDbContext();

        private FrameworkElement _infoPage;

        private Trip _selectedTrip;

        private List<Trip> _tripsOngoing = new List<Trip>();

        #endregion

        #region Constructors and Destructors

        public BoatsOutViewModel()
        {
            this.ParentAttached += (sender, e) =>
                {
                    this.TripsOngoing = this._db.Trip.Where(t => t.TripEndedTime == null).ToList();
                    this.UpdateInfo();
                };
        }

        #endregion

        #region Public Properties

        public BoatsOutAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as BoatsOutAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return this._infoPage ?? (this._infoPage = new BoatsOutAdditionalInfo());
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return this._selectedTrip;
            }

            set
            {
                this._selectedTrip = value;
                this.Notify();
                this.UpdateInfo();
            }
        }

        public List<Trip> TripsOngoing
        {
            get
            {
                return this._tripsOngoing;
            }

            set
            {
                this._tripsOngoing = value;
                this.Notify();
            }
        }

        #endregion

        // Methods
        #region Properties

        private IInfoContainerViewModel GetInfoContainerViewModel
        {
            get
            {
                return this.Parent as IInfoContainerViewModel;
            }
        }

        #endregion

        #region Methods

        private void UpdateInfo()
        {
            this.Info.SelectedTrip = this.SelectedTrip;

            this.GetInfoContainerViewModel.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}
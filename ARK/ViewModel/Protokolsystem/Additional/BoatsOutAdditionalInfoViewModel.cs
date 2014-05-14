using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using ARK.Model.DB;
using System;
namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BoatsOutAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private Trip _selectedTrip;
        DbArkContext db;

        // Constructor
        public BoatsOutAdditionalInfoViewModel()
        {
            db = DbArkContext.GetDbContext();

        }

        // Properties
        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set 
            { 
                _selectedTrip = value;
                if (_selectedTrip != null)
                    WarningSms = db.TripWarningSms.FirstOrDefault(t => t.Trip.Id == SelectedTrip.Id);
                
                Notify(); 
            }
        }

        private TripWarningSms _warningSms;

        public TripWarningSms WarningSms
        {
            get { return _warningSms; }
            set { _warningSms = value; }
        }

        public DateTime? SmsSentToBoat
        {
            get 
            {
                if (WarningSms != null)
                    return WarningSms.SentSms;
                else
                    return null;
            }
        }

        public DateTime? SmsRecievedFromBoat
        {
            get
            {
                if (WarningSms != null)
                    return WarningSms.RecievedSms;
                else
                    return null;
            }
        }

        public DateTime? SmsSentToAdministration
        {
            get
            {
                //if (WarningSms != null)
                //    return WarningSms.;
                //else
                    return null;
            }
        }
    }
}
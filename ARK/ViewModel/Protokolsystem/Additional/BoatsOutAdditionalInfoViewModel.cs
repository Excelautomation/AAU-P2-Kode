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
            set { _warningSms = value; NotifyCustom("SmsSentToBoat"); NotifyCustom("SmsRecievedFromBoat"); NotifyCustom("SmsSentToAdministration"); }
        }

        public DateTime? SmsSentToBoat
        {
            get 
            {
                if (WarningSms != null)
                {
                    return WarningSms.SentSms;
                }
                else
                    return null;
            }
        }

        public DateTime? SmsRecievedFromBoat
        {
            get
            {
                if (WarningSms != null)
                {
                    return WarningSms.RecievedSms;
                }
                else
                    return null;                
            }
        }

        public DateTime? SmsSentToAdministration
        {
            get
            {
                if (SmsSentToBoat != null)
                {
                    DateTime time = (DateTime)SmsSentToBoat;
                    if (DateTime.Compare(time, DateTime.Now.AddMinutes(-15)) < 0)
                    {
                        return SmsSentToBoat.Value.AddMinutes(15);
                    }
                    else 
                        return null;
                }
                else
                    return null;
            }
        }
    }
}
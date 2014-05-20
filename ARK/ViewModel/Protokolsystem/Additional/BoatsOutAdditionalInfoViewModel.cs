using System;
using System.Linq;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BoatsOutAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private Trip _selectedTrip;

        private TripWarningSms _warningSms;

        private DbArkContext db;

        // Constructor
        public BoatsOutAdditionalInfoViewModel()
        {
            db = DbArkContext.GetDbContext();
        }

        // Properties
        public Trip SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;
                if (_selectedTrip != null)
                {
                    WarningSms = db.TripWarningSms.FirstOrDefault(t => t.Trip.Id == SelectedTrip.Id);
                }

                Notify();
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
                {
                    return null;
                }
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
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
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
                {
                    return null;
                }
            }
        }

        public TripWarningSms WarningSms
        {
            get
            {
                return _warningSms;
            }

            set
            {
                _warningSms = value;
                NotifyCustom("SmsSentToBoat");
                NotifyCustom("SmsRecievedFromBoat");
                NotifyCustom("SmsSentToAdministration");
            }
        }
    }
}
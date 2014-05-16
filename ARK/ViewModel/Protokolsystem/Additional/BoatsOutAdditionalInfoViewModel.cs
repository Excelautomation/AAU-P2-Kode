using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BoatsOutAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        #region Fields

        private Trip _selectedTrip;

        private TripWarningSms _warningSms;

        private DbArkContext db;

        #endregion

        // Constructor
        #region Constructors and Destructors

        public BoatsOutAdditionalInfoViewModel()
        {
            this.db = DbArkContext.GetDbContext();
        }

        #endregion

        // Properties
        #region Public Properties

        public Trip SelectedTrip
        {
            get
            {
                return this._selectedTrip;
            }

            set
            {
                this._selectedTrip = value;
                if (this._selectedTrip != null)
                {
                    this.WarningSms = this.db.TripWarningSms.FirstOrDefault(t => t.Trip.Id == this.SelectedTrip.Id);
                }

                this.Notify();
            }
        }

        public DateTime? SmsRecievedFromBoat
        {
            get
            {
                if (this.WarningSms != null)
                {
                    return this.WarningSms.RecievedSms;
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
                if (this.SmsSentToBoat != null)
                {
                    DateTime time = (DateTime)this.SmsSentToBoat;
                    if (DateTime.Compare(time, DateTime.Now.AddMinutes(-15)) < 0)
                    {
                        return this.SmsSentToBoat.Value.AddMinutes(15);
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
                if (this.WarningSms != null)
                {
                    return this.WarningSms.SentSms;
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
                return this._warningSms;
            }

            set
            {
                this._warningSms = value;
                this.NotifyCustom("SmsSentToBoat");
                this.NotifyCustom("SmsRecievedFromBoat");
                this.NotifyCustom("SmsSentToAdministration");
            }
        }

        #endregion
    }
}
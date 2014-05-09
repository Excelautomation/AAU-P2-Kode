using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BoatsOutAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private Trip _selectedTrip;

        // Propertiesp
        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set { _selectedTrip = value; Notify(); }
        }
        private bool _smsSentToBoat;

        public bool SmsSentToBoat
        {
            get { return _smsSentToBoat; }
            set { _smsSentToBoat = value; Notify(); }
        }

        private bool _smsRecievedFromBoat;

        public bool SmsRecievedFromBoat
        {
            get { return _smsRecievedFromBoat; }
            set { _smsRecievedFromBoat = value; Notify(); }
        }
        private bool _smsSentToAdministration;

        public bool SmsSentToAdministration
        {
            get { return _smsSentToAdministration; }
            set { _smsSentToAdministration = value; Notify(); }
        }
    }
}
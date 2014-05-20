using System;

using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class TripViewModel : ViewModelBase
    {
        private Trip _trip;

        public TripViewModel(Trip trip)
        {
            Trip = trip;
        }

        public bool Editable
        {
            get
            {
                return Trip.TripEndedTime != null && (DateTime.Now - Trip.TripEndedTime.Value).TotalMinutes < 30;
            }
        }

        public Trip Trip
        {
            get
            {
                return _trip;
            }

            set
            {
                _trip = value;
                Notify();
            }
        }
    }
}
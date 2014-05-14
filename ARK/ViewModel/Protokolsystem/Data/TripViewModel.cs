using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class TripViewModel : ViewModelBase
    {
        private Trip _trip;

        public TripViewModel(Trip trip)
        {
            this.Trip = trip;
        }

        public Trip Trip
        {
            get { return _trip; }
            set
            {
                this._trip = value;
                Notify();
            }
        }

        public bool Editable
        {
            get
            {
                return this.Trip.TripEndedTime != null &&
                       (DateTime.Now - this.Trip.TripEndedTime.Value).TotalMinutes < 30;
            }
        }
    }
}

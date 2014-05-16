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
        #region Fields

        private Trip _trip;

        #endregion

        #region Constructors and Destructors

        public TripViewModel(Trip trip)
        {
            this.Trip = trip;
        }

        #endregion

        #region Public Properties

        public bool Editable
        {
            get
            {
                return this.Trip.TripEndedTime != null
                       && (DateTime.Now - this.Trip.TripEndedTime.Value).TotalMinutes < 30;
            }
        }

        public Trip Trip
        {
            get
            {
                return this._trip;
            }

            set
            {
                this._trip = value;
                this.Notify();
            }
        }

        #endregion
    }
}
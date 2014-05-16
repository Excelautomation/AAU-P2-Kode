using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class ChangeDistanceConfirmViewModel : ConfirmationViewModelBase
    {
        #region Fields

        private string _selectedDistance;

        private Trip _selectedTrip;

        #endregion

        // Fields
        #region Public Properties

        public ICommand Cancel
        {
            get
            {
                return this.GetCommand(this.Hide);
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                double tmp;
                return this.GetCommand(
                    e =>
                        {
                            this.SelectedTrip.Distance = double.Parse(this.SelectedDistance);
                            this.SelectedTrip.TripEndedTime = DateTime.Now;
                            DbArkContext.GetDbContext().SaveChanges();
                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        }, 
                    e =>
                    !string.IsNullOrEmpty(this.SelectedDistance) && double.TryParse(this.SelectedDistance, out tmp)
                    && double.Parse(this.SelectedDistance) > 0);
            }
        }

        public string SelectedDistance
        {
            get
            {
                return this._selectedDistance;
            }

            set
            {
                this._selectedDistance = value;
                this.Notify();
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

                if (this._selectedTrip != null)
                {
                    this.SelectedDistance = this._selectedTrip.Distance.ToString();
                }
            }
        }

        #endregion
    }
}
using System;
using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class ChangeDistanceConfirmViewModel : ConfirmationViewModelBase
    {
        private Trip _selectedTrip;
        private string _selectedDistance;
        // Fields

        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set
        {
                _selectedTrip = value; Notify();
        
                if (_selectedTrip != null)
                    SelectedDistance = _selectedTrip.Distance.ToString();
        }
        }
        
        public string SelectedDistance
        {
            get { return _selectedDistance; }
            set
            {
                _selectedDistance = value;
                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                double tmp;
                return GetCommand(e => 
                {
                    SelectedTrip.Distance = double.Parse(SelectedDistance);
                    SelectedTrip.TripEndedTime = DateTime.Now;
                        DbArkContext.GetDbContext().SaveChanges();
                    base.Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                }, e => !string.IsNullOrEmpty(SelectedDistance) 
                    && double.TryParse(SelectedDistance, out tmp) 
                    && double.Parse(SelectedDistance) > 0);
            }
        }

        public ICommand Cancel
        {
            get
            {
                return GetCommand(Hide);
            }
        }
    }
}
using System;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class ChangeDistanceConfirmViewModel : ConfirmationViewModelBase
    {
        private string _selectedDistance;

        private Trip _selectedTrip;

        // Fields
        public ICommand Cancel
        {
            get
            {
                return GetCommand(Hide);
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                double tmp;
                return GetCommand(
                    e =>
                        {
                            SelectedTrip.Distance = double.Parse(SelectedDistance);
                            SelectedTrip.TripEndedTime = DateTime.Now;
                            DbArkContext.GetDbContext().SaveChanges();
                            ProtocolSystem.UpdateDailyKilometers();
                            ProtocolSystem.UpdateNumBoatsOut();
                            Hide();
                            ProtocolSystem.StatisticsDistance.Execute(null);
                        }, 
                    e =>
                    !string.IsNullOrEmpty(SelectedDistance) && double.TryParse(SelectedDistance, out tmp)
                    && double.Parse(SelectedDistance) > 0);
            }
        }

        public string SelectedDistance
        {
            get
            {
                return _selectedDistance;
            }

            set
            {
                _selectedDistance = value;
                Notify();
            }
        }

        public Trip SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;
                Notify();

                if (_selectedTrip != null)
                {
                    SelectedDistance = _selectedTrip.Distance.ToString();
                }
            }
        }
    }
}
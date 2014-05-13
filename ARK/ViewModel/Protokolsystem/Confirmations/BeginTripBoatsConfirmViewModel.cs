using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class BeginTripBoatsConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private Trip _trip;
        
        
        //BeginTripBoatsConfirmViewModel()
        //{

        //}







        //_db.Trip.Add(trip);
        //_db.SaveChanges();

        //ResetData();

        public ICommand CloseWindow
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    
                });
            }
        }

        public Trip Trip
        {
            get { return _trip; }
            set { _trip = value; Notify(); }
        }
    }
}
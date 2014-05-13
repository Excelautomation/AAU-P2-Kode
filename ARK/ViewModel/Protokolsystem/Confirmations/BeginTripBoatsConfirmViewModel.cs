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

        public BeginTripBoatsConfirmViewModel()
        {
           
        }

        public ICommand CloseWindow
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    var mainViewModel = Parent as ProtocolSystemMainViewModel;
                    // En lorteløsning?
                    mainViewModel.HideDialog();
                });
            }
        }

        public Trip Trip
        {
            get { return _trip; }
            set { 
                _trip = value; 
                Notify(); 
            }
        }
    }
}
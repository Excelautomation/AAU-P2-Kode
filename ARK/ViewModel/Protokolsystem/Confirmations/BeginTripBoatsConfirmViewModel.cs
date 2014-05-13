using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
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

        public ICommand Save
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DbArkContext.GetDbContext().Trip.Add(Trip);
                    DbArkContext.GetDbContext().SaveChanges();

                    Hide();
                    ProtocolSystem.BoatsOut.Execute(null);
                });
            }
        }


        public ICommand CloseWindow
        {
            get
            {
                return GetCommand<object>(e => ProtocolSystem.HideDialog());
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
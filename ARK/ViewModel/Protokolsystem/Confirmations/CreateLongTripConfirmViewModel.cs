using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class CreateLongTripConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private LongTripForm _longTrip;

        public LongTripForm LongTrip 
        {
            get { return _longTrip; }
            set { _longTrip = value; Notify(); }
        }

        public ICommand SaveForm
        {
            get
            {
                return GetCommand(() =>
                {
                    DbArkContext.GetDbContext().LongTripForm.Add(LongTrip);
                    DbArkContext.GetDbContext().SaveChanges();
                    
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand CancelForm
        {
            get
            {
                return GetCommand(() =>
                {
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand ChangeForm
        {
            get
            {
                return GetCommand(() =>
                {
                    Hide();
                });
            }
        }
    }
}
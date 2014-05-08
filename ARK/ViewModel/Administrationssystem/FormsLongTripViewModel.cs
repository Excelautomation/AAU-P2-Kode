using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.Model.DB;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsLongTripViewModel : ContentViewModelBase
    {

        private readonly DbArkContext _dbArkContext;
        private LongTripForm _LongDistanceForm;
        private bool _RecentChange = false;
        
        public FormsLongTripViewModel ()
        {
            _dbArkContext = DbArkContext.GetDbContext();
        }

        public bool RecentChange
        {
            get { return _RecentChange; }
            set { _RecentChange = value; Notify(); }
        }

        public LongTripForm LongDistanceForm
        {
            get { return _LongDistanceForm; }
            set { _LongDistanceForm = value; Notify(); }
        }

        public ICommand AcceptForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LongDistanceForm.Status = LongTripForm.BoatStatus.Accepted;
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
                 });
            }
        }

        public ICommand RejectForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LongDistanceForm.Status = LongTripForm.BoatStatus.Denied;
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
                });
            }
        }






    }
}

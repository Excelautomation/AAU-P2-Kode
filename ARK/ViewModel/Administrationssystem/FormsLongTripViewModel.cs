using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.Model.DB;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsLongTripViewModel : ContentViewModelBase
    {

        private readonly DbArkContext _dbArkContext;
        private LongDistanceForm _LongDistanceForm;
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

        public LongDistanceForm LongDistanceForm { get { return _LongDistanceForm; } 
            set { 
                _LongDistanceForm = value; Notify(); 
            } }

        public ICommand AcceptForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LongDistanceForm.Status = LongDistanceForm.BoatStatus.Accepteret;
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
                    LongDistanceForm.Status = LongDistanceForm.BoatStatus.Afvist;
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
                });
            }
        }






    }
}

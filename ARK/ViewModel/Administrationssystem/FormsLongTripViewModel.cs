using System.Data.Entity;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsLongTripViewModel : ContentViewModelBase
    {
        private LongTripForm _longDistanceForm;
        private bool _recentChange;

        public bool RecentChange
        {
            get { return _recentChange; }
            set
            {
                _recentChange = value;
                Notify();
            }
        }

        public LongTripForm LongDistanceForm
        {
            get { return _longDistanceForm; }
            set
            {
                _longDistanceForm = value;
                Notify();
            }
        }

        public ICommand AcceptForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LongDistanceForm.Status = LongTripForm.BoatStatus.Accepted;

                    Save();
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
                    Save();
                });
            }
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(LongDistanceForm).State = EntityState.Modified;
                db.SaveChanges();
            }

            RecentChange = true;
        }
    }
}
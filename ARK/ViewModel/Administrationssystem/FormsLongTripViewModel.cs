using System.Data.Entity;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsLongTripViewModel : ContentViewModelBase
    {
        #region Fields

        private LongTripForm _longDistanceForm;

        private bool _recentChange;

        #endregion

        #region Public Properties

        public ICommand AcceptForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.LongDistanceForm.Status = LongTripForm.BoatStatus.Accepted;

                            this.Save();
                        });
            }
        }

        public LongTripForm LongDistanceForm
        {
            get
            {
                return this._longDistanceForm;
            }

            set
            {
                this._longDistanceForm = value;
                this.Notify();
            }
        }

        public bool RecentChange
        {
            get
            {
                return this._recentChange;
            }

            set
            {
                this._recentChange = value;
                this.Notify();
            }
        }

        public ICommand RejectForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.LongDistanceForm.Status = LongTripForm.BoatStatus.Denied;
                            this.Save();
                        });
            }
        }

        #endregion

        #region Methods

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(this.LongDistanceForm).State = EntityState.Modified;
                db.SaveChanges();
            }

            this.RecentChange = true;
        }

        #endregion
    }
}
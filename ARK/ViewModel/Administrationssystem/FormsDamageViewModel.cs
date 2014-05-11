using System.Data.Entity;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private bool _recentChange;
        private DamageForm _damageForm;

        public bool RecentChange
        {
            get { return _recentChange; }
            set
            {
                _recentChange = value;
                Notify();
            }
        }

        public DamageForm DamageForm
        {
            get { return _damageForm; }
            set
            {
                _damageForm = value;
                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get { return GetCommand<object>(e => Save()); }
        }

        public ICommand DeactivateBåd
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Boat.Active = false;

                    Save();
                    // DamageformIndex = 0 // så den næste damageform vælges, men dette kan kun gøres når/hvis den nuværende damageform forsvinder fra listen.
                    // Det skal helst være sådan at closed damageforms ikke vises i listen, medmindre det er valgt. Så denne knap skal også få den pågænldende damageform til at forsvinde i listen.
                });
            }
        }

        public ICommand ActivateBåd
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Boat.Active = true;

                    Save();
                    // DamageformIndex = 0 // så den næste damageform vælges, men dette kan kun gøres når/hvis den nuværende damageform forsvinder fra listen.
                    // Det skal helst være sådan at closed damageforms ikke vises i listen, medmindre det er valgt. Så denne knap skal også få den pågænldende damageform til at forsvinde i listen.
                });
            }
        }

        public ICommand CloseForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Closed = true;

                    Save();
                    // DamageformIndex = 0 // så den næste damageform vælges, men dette kan kun gøres når/hvis den nuværende damageform forsvinder fra listen.
                    // Det skal helst være sådan at closed damageforms ikke vises i listen, medmindre det er valgt. Så denne knap skal også få den pågænldende damageform til at forsvinde i listen.
                });
            }
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(DamageForm).State = EntityState.Modified;
                db.SaveChanges();
            }

            RecentChange = true;
        }
    }
}
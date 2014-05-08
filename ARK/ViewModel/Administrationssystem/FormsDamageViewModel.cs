using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private readonly DbArkContext _dbArkContext;
        private bool _RecentChange;
        private DamageForm _damageForm;

        public FormsDamageViewModel()
        {
            _dbArkContext = DbArkContext.GetDbContext();
        }

        public bool RecentChange
        {
            get { return _RecentChange; }
            set { _RecentChange = value; Notify(); }
        }

        public DamageForm DamageForm
        {
            get { return _damageForm; }
            set
            { _damageForm = value; Notify(); }
        }

        public ICommand SaveChanges
        {
            get {
                return GetCommand<object>(e =>
                {
                    _dbArkContext.SaveChanges();
                    RecentChange = true;
                });
            }
        }

        public ICommand DeactivateBåd
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Boat.Active = false;
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
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
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
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
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
                    // DamageformIndex = 0 // så den næste damageform vælges, men dette kan kun gøres når/hvis den nuværende damageform forsvinder fra listen.
                    // Det skal helst være sådan at closed damageforms ikke vises i listen, medmindre det er valgt. Så denne knap skal også få den pågænldende damageform til at forsvinde i listen.
                });
            }
        }
    }
}
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private readonly DbArkContext _dbArkContext;
        private DamageForm _damageForm;
        private bool _RecentChange = false;

        public FormsDamageViewModel ()
        {
            _dbArkContext = DbArkContext.GetDbContext();



        }

        public bool RecentChange
        {
            get { return _RecentChange; }
            set { _RecentChange = value; Notify(); }
        }

        public DamageForm DamageForm { get { return _damageForm; } 
            set { 
                _damageForm = value; Notify(); 
            } }

        public ICommand DeaktiverBåd
        {
            get
            {
                return GetCommand<object>(e => 
                {  
                    DamageForm.Boat.Active = false;
                    DamageForm.Closed = true;
                    RecentChange = true;
                    _dbArkContext.SaveChanges();
                    // DamageformIndex = 0 // så den næste damageform vælges, men dette kan kun gøres når/hvis den nuværende damageform forsvinder fra listen.
                    // Det skal helst være sådan at closed damageforms ikke vises i listen, medmindre det er valgt. Så denne knap skal også få den pågænldende damageform til at forsvinde i listen.
                });
            }
        }

        public ICommand AktiverBåd
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Boat.Active = true;
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

using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private DamageForm _damageForm;
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
                });
            }
        }
    }
}

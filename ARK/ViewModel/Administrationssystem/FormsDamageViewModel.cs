using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        public DamageForm DamageForm { get; set; }

        public ICommand DeaktiverBåd
        {
            get
            {
                return GetCommand<DamageForm>(e => 
                { 
                    DamageForm = e; 
                    DamageForm.Boat.Active = false; 
                });
            }
        }

        public ICommand AktiverBåd
        {
            get
            {
                return GetCommand<DamageForm>(e =>
                {
                    DamageForm = e;
                    DamageForm.Boat.Active = true;
                });
            }
        }
    }
}

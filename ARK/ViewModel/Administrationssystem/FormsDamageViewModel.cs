using System.Windows.Input;
using ARK.Model;

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
                    DamageForm.Boat.Usable = false;
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
                    DamageForm.Boat.Usable = true;
                });
            }
        }
    }
}

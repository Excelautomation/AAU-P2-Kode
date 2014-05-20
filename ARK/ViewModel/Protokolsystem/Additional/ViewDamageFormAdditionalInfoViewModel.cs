using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class ViewDamageFormAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private DamageForm _selectedDamageForm;

        // Properties
        public DamageForm SelectedDamageForm
        {
            get
            {
                return _selectedDamageForm;
            }

            set
            {
                _selectedDamageForm = value;
                Notify();
            }
        }
    }
}
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class MembersInformationAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private Member _selectedMember;

        // Properties
        public Member SelectedMember
        {
            get
            {
                return _selectedMember;
            }

            set
            {
                _selectedMember = value;
                Notify();
            }
        }
    }
}
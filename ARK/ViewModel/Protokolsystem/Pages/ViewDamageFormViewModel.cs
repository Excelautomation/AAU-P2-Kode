using System.Windows.Input;
using ARK.View.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class ViewDamageFormViewModel : ProtokolsystemContentViewModelBase
    {


        public ICommand CreateDamageForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "AKTIVE SKADES BLANKETTER"));
            }
        }
    }
}

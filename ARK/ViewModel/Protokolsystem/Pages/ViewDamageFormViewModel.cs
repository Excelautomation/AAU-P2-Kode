using ARK.Protokolsystem.Pages;
using ARK.View.Protokolsystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
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

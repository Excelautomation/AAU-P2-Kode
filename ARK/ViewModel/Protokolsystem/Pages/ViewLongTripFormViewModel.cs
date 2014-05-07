using ARK.Protokolsystem.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    class ViewLongTripFormViewModel : ProtokolsystemContentViewModelBase
    {


        public ICommand CreateLongTripForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new CreateLongTripForm(), "OPRET NY LANGTUR"));
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new ViewLongTripForm(), "AKTIVE LANGTURS BLANKETTER"));
            }
        }
    }
}

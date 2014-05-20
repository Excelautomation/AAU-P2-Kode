using System.Windows.Input;

using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtokolsystemContentViewModelBase : ContentViewModelBase
    {
        public ProtokolsystemContentViewModelBase()
        {
            ParentAttached += (sender, args) => NotifyCustom("ProtocolSystem");
        }

        public ProtocolSystemMainViewModel ProtocolSystem
        {
            get
            {
                return Parent as ProtocolSystemMainViewModel;
            }
        }

        public ICommand ToggleFilter
        {
            get
            {
                return GetCommand(e => { ProtocolSystem.EnableFilters = !ProtocolSystem.EnableFilters; });
            }
        }

        public ICommand ToggleKeyboard
        {
            get
            {
                return GetCommand(e => { ProtocolSystem.EnableSearch = !ProtocolSystem.EnableSearch; });
            }
        }
    }
}
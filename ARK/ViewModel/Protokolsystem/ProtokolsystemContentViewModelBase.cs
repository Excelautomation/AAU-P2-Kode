using System.Windows.Input;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

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
            get { return Parent as ProtocolSystemMainViewModel; }
        }

        public ICommand ToggleKeyboard
        {
            get { return GetCommand<object>(e => { ProtocolSystem.EnableSearch = !ProtocolSystem.EnableSearch; }); }
        }

        public ICommand ToggleFilter
        {
            get { return GetCommand<object>(e => { ProtocolSystem.EnableFilters = !ProtocolSystem.EnableFilters; }); }
        }
    }
}
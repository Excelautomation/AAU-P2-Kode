using System.Windows.Input;

using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem
{
    public class ProtokolsystemContentViewModelBase : ContentViewModelBase
    {
        #region Constructors and Destructors

        public ProtokolsystemContentViewModelBase()
        {
            this.ParentAttached += (sender, args) => this.NotifyCustom("ProtocolSystem");
        }

        #endregion

        #region Public Properties

        public ProtocolSystemMainViewModel ProtocolSystem
        {
            get
            {
                return this.Parent as ProtocolSystemMainViewModel;
            }
        }

        public ICommand ToggleFilter
        {
            get
            {
                return this.GetCommand(e => { this.ProtocolSystem.EnableFilters = !this.ProtocolSystem.EnableFilters; });
            }
        }

        public ICommand ToggleKeyboard
        {
            get
            {
                return this.GetCommand(e => { this.ProtocolSystem.EnableSearch = !this.ProtocolSystem.EnableSearch; });
            }
        }

        #endregion
    }
}
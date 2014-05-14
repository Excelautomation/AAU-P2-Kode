using System;
using System.Windows;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    public class ConfirmationViewModelBase : ProtokolsystemContentViewModelBase
    {
        public ICommand CommandHide
        {
            get { return GetCommand(e => Hide()); }
        }

        public virtual void Hide()
        {
            ProtocolSystem.HideDialog();

            if (WindowHide != null)
                WindowHide(this, new EventArgs());
        }

        public event EventHandler WindowHide;
    }
}

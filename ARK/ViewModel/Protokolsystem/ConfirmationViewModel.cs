using System;
using System.Windows;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    public class ConfirmationViewModelBase : ProtokolsystemContentViewModelBase
    {
        #region Public Events

        public event EventHandler WindowHide;

        #endregion

        #region Public Properties

        public ICommand CommandHide
        {
            get
            {
                return this.GetCommand(e => this.Hide());
            }
        }

        #endregion

        #region Public Methods and Operators

        public virtual void Hide()
        {
            this.ProtocolSystem.HideDialog();

            if (this.WindowHide != null)
            {
                this.WindowHide(this, new EventArgs());
            }
        }

        #endregion
    }
}
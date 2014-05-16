using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class ViewModelBase : IViewModelBase
    {
        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        protected ICommand GetCommand(Action executeMethod)
        {
            return this.GetCommand(e => executeMethod());
        }

        protected ICommand GetCommand(Action executeMethod, Func<bool> canExecute)
        {
            return this.GetCommand(e => executeMethod(), e => canExecute());
        }

        protected ICommand GetCommand(Action<object> executeMethod)
        {
            return new RelayCommand(executeMethod);
        }

        protected ICommand GetCommand(Action<object> executeMethod, Predicate<object> canExecute)
        {
            return new RelayCommand(executeMethod, canExecute);
        }

        protected void Notify([CallerMemberName] string propertyName = "")
        {
            this.NotifyCustom(propertyName);
        }

        protected void NotifyCustom(string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
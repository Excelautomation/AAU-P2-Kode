using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public abstract class ViewModelBase : IViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propertyName = "")
        {
            NotifyCustom(propertyName);
        }

        protected void NotifyCustom(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected ICommand GetCommand<T>(Action<T> executeMethod)
        {
            return GetCommand(executeMethod, e => true);
        }

        protected ICommand GetCommand<T>(Action<T> executeMethod, Func<T, bool> canExecute)
        {
            return new DelegateCommand<T>(executeMethod, canExecute);
        }
    }
}
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
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

        protected ICommand GetCommand(Action executeMethod)
        {
            return GetCommand(e => executeMethod());
        }

        protected ICommand GetCommand(Action<object> executeMethod)
        {
            return new RelayCommand(executeMethod);
        }

        protected ICommand GetCommand(Action<object> executeMethod, Predicate<object> canExecute)
        {
            return new RelayCommand(executeMethod, canExecute);
        }
    }
}
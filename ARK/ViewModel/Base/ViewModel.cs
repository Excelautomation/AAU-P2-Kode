using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected ICommand GetCommand<T>(Action<T> executeMethod)
        {
            return GetCommand(executeMethod, (e) => true);
        }

        protected ICommand GetCommand<T>(Action<T> executeMethod, Func<T, bool> canExecute)
        {
            return new DelegateCommand<T>(executeMethod, canExecute);
        }
    }
}
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propertyName = "")
        {
            NotifyProp(propertyName);
        }

        protected void NotifyProp(string propertyName = "")
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
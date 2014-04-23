using System;
using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public Action<T> executeMethod { get; set; }
        public Func<T, bool> canExecuteMethod { get; set; }

        public bool CanExecute(object parameter)
        {
            return canExecuteMethod((T)parameter);
        }

        public void Execute(object parameter)
        {
            executeMethod((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}

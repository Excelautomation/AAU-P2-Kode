using System;
using System.Windows.Input;

namespace ARK.ViewModel
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
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecuteMethod((T) parameter);
        }

        public void Execute(object parameter)
        {
            executeMethod((T) parameter);
        }

        public virtual void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
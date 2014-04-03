using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARK.ViewModel
{
    public class DelegateCommand<T> : ICommand
    {
        public Action<T> executeMethod { get; set; }
        public Func<T, bool> canExecuteMethod { get; set; }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

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

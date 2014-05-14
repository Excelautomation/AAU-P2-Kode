using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ARK.ViewModel.Base
{
    /// <summary>
    /// Implementation of the ICommand interface
    /// </summary>
    /// <remarks>
    /// This implementation of RelayCommand was created by Josh Smith http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </remarks>
    public class RelayCommand : ICommand 
    { 
        readonly Action<object> _execute; 
        readonly Predicate<object> _canExecute; 

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute"); 
            _execute = execute; 
            _canExecute = canExecute; 
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        } 
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; } 
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }


    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null) throw new ArgumentNullException("execute");
            _execute = executeMethod;
            _canExecute = canExecuteMethod;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}

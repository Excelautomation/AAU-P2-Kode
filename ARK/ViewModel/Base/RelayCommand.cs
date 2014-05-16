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
        #region Fields

        private readonly Predicate<object> _canExecute;

        private readonly Action<object> _execute;

        #endregion

        #region Constructors and Destructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }

        #endregion

        #region Public Events

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Methods and Operators

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        #endregion
    }
}
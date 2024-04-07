using System;
using System.Windows.Input;

namespace SmartSchoolboyApp.Helpers
{
    public class DelegateCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;

        readonly Predicate<object> _canExecute;

        #endregion

        #region Constract

        public DelegateCommand(Action<object> execute):this(execute, null)  { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute is null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand members

        public bool CanExecute(object parametr)
        {
            return _canExecute?.Invoke(parametr) ?? true;
        }

        public void Execute(object parametr)
        {
            _execute(parametr);
        }

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
    }
}

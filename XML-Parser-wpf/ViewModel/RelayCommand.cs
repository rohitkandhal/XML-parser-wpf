using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Test1.ViewModel
{
    /// <summary>
    /// Custom relay command implementation
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action<object> _action;

        public RelayCommand(Action<object> action)
        {
            this._action = action;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

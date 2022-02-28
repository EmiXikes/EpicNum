using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EpicNum.ViewModel
{
    public class RCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object> _actionToExecute;
        private readonly Func<object, bool> _canExecute;

        public RCommand(Action<object> ActionToExecute, Func<object, bool> CanExecute)
        {
            _actionToExecute = ActionToExecute;
            _canExecute = CanExecute;
        }

        public RCommand(Action<object> ActionToExecute)
        {
            _actionToExecute = ActionToExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);

            //    Same as this:

            //if (_canExecute == null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return _canExecute(parameter);
            //}
        }

        public void Execute(object parameter)
        {
            _actionToExecute(parameter);
        }
    }
}

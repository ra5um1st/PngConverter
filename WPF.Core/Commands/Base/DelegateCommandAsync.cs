using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.Core.Commands
{
    public class DelegateCommandAsync : ICommand
    {
        public DelegateCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected Func<object, Task> execute;
        protected Func<object, bool> canExecute;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            protected set
            {
                _isExecuting = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke(parameter) && !_isExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            await execute?.Invoke(parameter);
            IsExecuting = false;
        }
    }
}

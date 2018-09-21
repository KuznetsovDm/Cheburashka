using System;
using System.Windows.Input;

namespace Cheburashka
{
    class SimpleCommand<T> : ICommand
    {
        readonly Action<T> onExecute;
        public SimpleCommand(Action<T> onExecute) { this.onExecute = onExecute; }

        public event EventHandler CanExecuteChanged;
        private bool canExecute = true;
        public bool SetCanExecute
        {
            get => canExecute;
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => onExecute((T)parameter);
    }

    class SimpleCommand : ICommand
    {
        readonly Action onExecute;
        public SimpleCommand(Action onExecute) { this.onExecute = onExecute; }

        public event EventHandler CanExecuteChanged;
        private bool canExecute = true;
        public bool SetCanExecute
        {
            get => canExecute;
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => onExecute();
    }
}

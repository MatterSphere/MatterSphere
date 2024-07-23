using System;
using System.Windows.Input;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class Command<T> : ICommand
    {


        private Action<T> onExecute;
        private Func<T, bool> canExecuteMethod;
        public Command(Action<T> onExecute)
            : this(onExecute, null)
        {
        }
        public Command(Action<T> onExecute, Func<T, bool> canExecuteMethod)
        {
            this.onExecute = onExecute;
            this.canExecuteMethod = canExecuteMethod;
        }

        bool lastResult;
        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod == null)
                return true;

            bool res = canExecuteMethod((T)parameter);

            if (lastResult != res)
            {
                lastResult = res;
                RaiseCanExecuteChanged();
            }

            return res;
        }

        public event EventHandler CanExecuteChanged;
        private void RaiseCanExecuteChanged()
        {
            var ev = CanExecuteChanged;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            this.onExecute((T)parameter);
        }
    }
}

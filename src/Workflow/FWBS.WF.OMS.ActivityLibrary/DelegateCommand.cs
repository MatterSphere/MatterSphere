#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public class DelegateCommand : System.Windows.Input.ICommand
	{
		#region Fields
		private readonly Predicate<object> canExecute;
		private readonly Action<object> execute;

		public event EventHandler CanExecuteChanged;
		#endregion

		#region Constructors
		public DelegateCommand(Action<object> execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action<object> execute,
					   Predicate<object> canExecute)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}
		#endregion

		#region CanExecute
		public bool CanExecute(object parameter)
		{
			if (this.canExecute == null)
			{
				return true;
			}

			return this.canExecute(parameter);
		}
		#endregion

		#region Execute
		public void Execute(object parameter)
		{
			this.execute(parameter);
		}
		#endregion

		#region RaiseCanExecuteChanged
		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}

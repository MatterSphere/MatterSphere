#region References
using System.Collections.Generic;
using System.Windows;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    #region SynchonisationWindow
    internal partial class SynchonisationWindow : Window
	{
		#region Fields
		/// <summary>
		///  The workflow codes that can be added
		/// </summary>
		private SortedObservableCollection<string> _Codes = null;
		/// <summary>
		/// The workflow codes to add
		/// </summary>
		private SortedObservableCollection<string> _CodesToAdd = new SortedObservableCollection<string>();
		#endregion

		#region Constructors
		private SynchonisationWindow()
		{
			InitializeComponent();

			this.Title = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAISW0001", "Select Workflow To Add", "").Text;
			this.buttonOK.Content = FWBS.OMS.Session.CurrentSession.Resources.GetResource("OK", "OK", "").Text;
			this.buttonCancel.Content = FWBS.OMS.Session.CurrentSession.Resources.GetResource("CANCEL", "Cancel", "").Text;
		}

		internal SynchonisationWindow(HashSet<string> workflowCodes)
			: this()
		{
			this._Codes = new SortedObservableCollection<string>(workflowCodes);

			this.listBoxAll.ItemsSource = this._Codes;
			this.listBoxSelected.ItemsSource = this._CodesToAdd;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The workflow codes that the user has selected to add
		/// </summary>
		internal HashSet<string> Selected
		{
			get
			{
				HashSet<string> retValue = new HashSet<string>();
				foreach (string code in this._CodesToAdd)
				{
					retValue.Add(code);
				}
				return retValue;
			}
		}
		#endregion

		#region Loaded event
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		}
		#endregion

		#region OK button click
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		#endregion

		#region buttonAdd_Click
		private void buttonAdd_Click(object sender, RoutedEventArgs e)
		{
			for (int i = this.listBoxAll.SelectedItems.Count - 1; i >= 0; i--)
			{
				string code = (string)this.listBoxAll.SelectedItems[i];
				this._Codes.Remove(code);
				this._CodesToAdd.Add(code);
			}
		}
		#endregion

		#region buttonRemove_Click
		private void buttonRemove_Click(object sender, RoutedEventArgs e)
		{
			for (int i = this.listBoxSelected.SelectedItems.Count - 1; i >= 0; i--)
			{
				string code = (string)this.listBoxSelected.SelectedItems[i];
				this._CodesToAdd.Remove(code);
				this._Codes.Add(code);
			}
		}
		#endregion
	}
	#endregion
}

#region References
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    internal partial class ScriptsWindow : Window
	{
		#region Constructors
		internal ScriptsWindow()
		{
			InitializeComponent();
		}

		internal ScriptsWindow(DataTable currentList)
			: this()
		{
			// get list from database
			try
			{
				System.Data.DataTable dt = Script.ScriptGen.GetScripts(FWBS.OMS.Workflow.Constants.SCRIPTTYPE);
			
				HashSet<string> scriptCodes = new HashSet<string>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					// There is only a single column which is the code
					string code = dt.Rows[i][0].ToString();
					DataRow[] rows = currentList.Select(string.Format("Name = '{0}'", code));
					if (rows.Length < 1)
					{
						// it is not in the current list, add ...
						scriptCodes.Add(code);
					}
				}
				this.listBoxScripts.ItemsSource = scriptCodes;
			}
			catch (Exception ex)
			{
				// Any exception display message and exit
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
				this.DialogResult = false;
			}
		}	
		#endregion

		#region Properties
		internal HashSet<string> ScriptCodes
		{
			get
			{
				HashSet<string> selectedItems = new HashSet<string>();
				foreach (string str in this.listBoxScripts.SelectedItems)
				{
					selectedItems.Add(str);
				}
				return selectedItems;
			}
		}
		#endregion

		#region OK button click
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		#endregion
	}
}

using System.Windows;

namespace FWBS.OMS.Workflow.Admin
{
    internal partial class CodeDescriptionWindow : Window
	{
		#region Constructor
		internal CodeDescriptionWindow()
		{
			InitializeComponent();
		}
		#endregion

		#region Properties
		internal string Code
		{
			get { return this.textBoxCode.Text.Trim(); }
		}

		#endregion

		#region OK button event handler
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			// Exit only if we have some code!
			if (!string.IsNullOrEmpty(this.textBoxCode.Text.Trim()))
			{
				this.DialogResult = true;
			}
		}
		#endregion
	}
}

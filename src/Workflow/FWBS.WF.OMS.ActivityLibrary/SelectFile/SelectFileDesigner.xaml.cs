#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SelectFileDesigner
	{
		#region Constructor
		public SelectFileDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(SelectFileDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region SelectFileDesigner_Loaded
		void SelectFileDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSLFILBDTT", "Select a File Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSLFILOTTT", "A File output result", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}			
		}
		#endregion
	}
}

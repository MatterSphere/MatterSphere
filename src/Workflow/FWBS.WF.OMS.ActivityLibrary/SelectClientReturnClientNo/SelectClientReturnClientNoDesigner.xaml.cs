#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SelectClientReturnClientNoDesigner
	{
		#region Constructor
		public SelectClientReturnClientNoDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(SelectClientReturnClientNoDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region SelectClientReturnClientNoDesigner_Loaded
		void SelectClientReturnClientNoDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSCRCNBDTT", "Select a Client's return Number Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSCRCNOTTT", "A string output result", "").Text;
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

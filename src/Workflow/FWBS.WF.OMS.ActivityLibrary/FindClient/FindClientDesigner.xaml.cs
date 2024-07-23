#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class FindClientDesigner
	{
		#region Constructor
		public FindClientDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(FindClientDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region FindClientDesigner_Loaded
		void FindClientDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFICLBDTT", "Find a Client Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFICLOTTT", "A Client output result", "").Text;
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

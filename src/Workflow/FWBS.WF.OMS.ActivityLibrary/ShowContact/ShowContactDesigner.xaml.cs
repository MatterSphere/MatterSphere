#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowContactDesigner
	{
		#region Constructor
		public ShowContactDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowContactDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region ShowContactDesigner_Loaded
		void ShowContactDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWCONBDTT", "Show a Contact Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWCONINTT", "A required Contact as input", "").Text;
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

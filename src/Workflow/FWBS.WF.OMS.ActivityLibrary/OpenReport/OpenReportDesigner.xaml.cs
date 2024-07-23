#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class OpenReportDesigner
	{
		#region Constructor
		public OpenReportDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(OpenReportDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region OpenReportDesigner_Loaded
		void OpenReportDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTOPREBDTT", "Open a Report Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTOPREINTT", "A required Report Code String as input", "").Text;
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

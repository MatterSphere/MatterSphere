#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class WriteToEventLogDesigner
	{
		#region Constructor
		public WriteToEventLogDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(WriteToEventLogDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string InTooltip3 { get; set; }
		public string InTooltip4 { get; set; }
		public string InTooltip5 { get; set; }
		#endregion

		#region WriteToEventLogDesigner_Loaded
		void WriteToEventLogDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELBDTT", "Write to Event Log (requires priviliges)", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELINTT", "A Message string as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELINTT2", "An Event Log Source string as input", "").Text;
				this.InTooltip3 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELINTT3", "An Event Log Type as input", "").Text;
				this.InTooltip4 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELINTT4", "An Event Category as input", "").Text;
				this.InTooltip5 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTWTELINTT5", "An Event ID as input", "").Text;
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

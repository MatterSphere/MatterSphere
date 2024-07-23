#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class CreateTimeRecordDesigner
	{
		#region Constructor
		public CreateTimeRecordDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreateTimeRecordDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string InTooltip3 { get; set; }
		public string InTooltip4 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region CreateTimeRecordDesigner_Loaded
		void CreateTimeRecordDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTRBDTT", "Create a new Time Record", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTRINTT", "A required OMSFile as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTROTTT", "A boolean output result", "").Text;
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

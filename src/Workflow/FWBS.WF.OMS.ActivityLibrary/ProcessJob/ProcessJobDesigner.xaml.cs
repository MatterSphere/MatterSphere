#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class ProcessJobDesigner
	{
		#region Constructor
		public ProcessJobDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ProcessJobDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region ProcessJobDesigner_Loaded
		void ProcessJobDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPRJOBDTT", "Process a Job Activity", "").Text;
                this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPRJOINTT", "A Precedent Job as input", "").Text;			
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPRJOOTTT", "A Process Job Status output result", "").Text;
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

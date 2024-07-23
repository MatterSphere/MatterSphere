#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class RunPrecedentDesigner
	{
		#region Constructor
		public RunPrecedentDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(RunPrecedentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string InTooltip3 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region RunPrecedentDesigner_Loaded
		void RunPrecedentDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTTMPSBDTT", "Start a Template Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTTMPSINTT", "An IOMS App as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTTMPSINTT2", "A Precedent as input", "").Text;
				this.InTooltip3 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTTMPSINTT3", "An Associate as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTTMPSOTTT", "A Precedent Job output result", "").Text;
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

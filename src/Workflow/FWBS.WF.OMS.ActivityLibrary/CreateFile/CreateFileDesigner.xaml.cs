#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class CreateFileDesigner
	{
		#region Constructor
		public CreateFileDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreateFileDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region CreateFileDesigner_Loaded
		void CreateFileDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCFILBDTT", "Create a new File Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCFILINTT", "A required Client as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCFILOTTT", "A File output result", "").Text;
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

#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class AddFileEventDesigner
	{
		#region Constructor
		public AddFileEventDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(AddFileEventDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string InTooltip3 { get; set; }
		#endregion

		#region AddFileEventDesigner_Loaded
		void AddFileEventDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFEBDTT", "Add a File Event Activity", "").Text;
                this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFEINTT", "A required File as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFEINTT2", "An Event Type string as input", "").Text;
				this.InTooltip3 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFEINTT3", "An Event Description string as input", "").Text;				
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

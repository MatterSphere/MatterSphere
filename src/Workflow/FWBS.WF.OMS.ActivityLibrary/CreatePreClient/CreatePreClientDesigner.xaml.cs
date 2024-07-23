#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class CreatePreClientDesigner
	{
		#region Consructor
		public CreatePreClientDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreatePreClientDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region CreatePreClientDesigner_Loaded
		void CreatePreClientDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCPCLBDTT", "Create a new Pre-Client Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCPCLOTTT", "A boolean indicating if the activity was successful", "").Text;
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

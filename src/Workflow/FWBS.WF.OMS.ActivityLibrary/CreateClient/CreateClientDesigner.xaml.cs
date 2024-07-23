#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	// Interaction logic for CreateClientDesigner.xaml
	internal sealed partial class CreateClientDesigner
	{
		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region 
		public CreateClientDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreateClientDesigner_Loaded);
		}
		#endregion

		#region 
		void CreateClientDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCCLIBDTT", "Create a new Client Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCCLITTT", "A Client output result", "").Text;
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

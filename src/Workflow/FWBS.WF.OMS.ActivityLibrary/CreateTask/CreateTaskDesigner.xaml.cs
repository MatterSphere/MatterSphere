#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class CreateTaskDesigner
	{
		#region Constructor
		public CreateTaskDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreateTaskDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region CreateTaskDesigner_Loaded
		void CreateTaskDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTASBDTT", "Create a new Task Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTASINTT", "An optional File as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCTASOTTT", "A Task output result", "").Text;
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

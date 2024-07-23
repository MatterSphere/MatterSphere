#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SelectAssociateDesigner
	{
		#region Constructor
		public SelectAssociateDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(SelectAssociateDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region SelectAssociateDesigner_Loaded
		void SelectAssociateDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSEASSBDTT", "Select an Associate Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSEASSOTTT", "An Associate output result", "").Text;
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

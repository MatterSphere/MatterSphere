#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowFileConflictDesigner
	{
		#region Constructor
		public ShowFileConflictDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowFileConflictDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region ShowFileConflictDesigner_Loaded
		void ShowFileConflictDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWFCNBDTT", "Show a File Conflict", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWFCNINTT", "A required File as input", "").Text;
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

#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class AddFileNoteDesigner
	{
		#region Constructor
		public AddFileNoteDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(AddFileNoteDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		#endregion

		#region AddFileNoteDesigner_Loaded
		void AddFileNoteDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFNBDTT", "Add a File Note Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFNINTT", "A required File as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTADDFNINTT2", "A Note string as input", "").Text;
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

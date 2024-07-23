#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowDocumentDesigner
	{
		#region Constructor
		public ShowDocumentDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowDocumentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region ShowDocumentDesigner_Loaded
		void ShowDocumentDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWDOCBDTT", "Show a Document", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWDOCINTT", "A required Document as input", "").Text;
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

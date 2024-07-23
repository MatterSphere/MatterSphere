using System.Windows.Forms.Integration;

namespace FWBS.OMS.Workflow.Admin
{
    public class WindowsFormsWFControl : ElementHost
	{
		#region Constructor 
		public WindowsFormsWFControl()
		{
			this.Child = new WFControl();
		}
		#endregion
	}
}

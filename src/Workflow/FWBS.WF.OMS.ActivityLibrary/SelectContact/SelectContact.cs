#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectContact.SelectContactToolboxIcon.bmp")]
	[Description("Selects a Contact")]
	[Designer(typeof(SelectContactDesigner))]
	public sealed class SelectContact : CodeActivity<FWBS.OMS.Contact>
	{
		#region Arguments
		#endregion

		#region Constructor
		public SelectContact()
		{
			this.DisplayName = "Select Contact";
		}
		#endregion

		#region Execute
		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override FWBS.OMS.Contact Execute(CodeActivityContext context)
		{
			return FWBS.OMS.UI.Windows.Services.Searches.FindContact();
		}
		#endregion
	}
}

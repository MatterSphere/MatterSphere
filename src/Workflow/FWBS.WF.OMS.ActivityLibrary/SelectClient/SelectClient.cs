#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectClient.SelectClientToolboxIcon.bmp")]
	[Description("Selects a Client")]
	[Designer(typeof(SelectClientDesigner))]
	public sealed class SelectClient : CodeActivity<FWBS.OMS.Client>
	{
		#region Arguments
		#endregion

		#region Constructor
		public SelectClient()
		{
			this.DisplayName = "Select Client";
		}
		#endregion

		#region Execute
		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override FWBS.OMS.Client Execute(CodeActivityContext context)
		{
			return FWBS.OMS.UI.Windows.Services.SelectClient();
		}
		#endregion
	}
}

#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectAssociate.SelectAssociateToolboxIcon.bmp")]
	[Description("Selects an Associate")]
	[Designer(typeof(SelectAssociateDesigner))]
	public sealed class SelectAssociate : CodeActivity<FWBS.OMS.Associate>
	{
		#region Constructor
		public SelectAssociate()
		{
			this.DisplayName = "Select Associate";
		}
		#endregion

		#region Arguments
		#endregion

		#region Execute
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Associate Execute(CodeActivityContext context)
		{
			return FWBS.OMS.UI.Windows.Services.SelectAssociate();
		}
		#endregion
	}
}

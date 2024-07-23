#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectFile.SelectFileToolboxIcon.bmp")]
	[Description("Selects a File")]
	[Designer(typeof(SelectFileDesigner))]
	public sealed class SelectFile : CodeActivity<FWBS.OMS.OMSFile>
	{
		#region Arguments
		#endregion

		#region Constructor
		public SelectFile()
		{
			this.DisplayName = "Select File";
		}
		#endregion

		#region Execute
		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override FWBS.OMS.OMSFile Execute(CodeActivityContext context)
		{
			return FWBS.OMS.UI.Windows.Services.SelectFile();
		}
		#endregion
	}
}

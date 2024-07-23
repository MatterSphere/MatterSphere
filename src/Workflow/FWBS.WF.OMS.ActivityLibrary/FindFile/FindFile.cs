#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindFile.FindFileToolboxIcon.bmp")]
    [Description("Find a File")]
    [Designer(typeof(FindFileDesigner))]
    public sealed class FindFile : CodeActivity<FWBS.OMS.OMSFile>
    {
        #region Arguments
        #endregion

        #region Constructor
        public FindFile()
        {
            this.DisplayName = "Find File";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override FWBS.OMS.OMSFile Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.Searches.FindFile();
        }
        #endregion
    }
}

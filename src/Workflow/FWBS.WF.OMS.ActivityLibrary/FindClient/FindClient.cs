#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindClient.FindClientToolboxIcon.bmp")]
    [Description("Find a Client")]
    [Designer(typeof(FindClientDesigner))]
    public sealed class FindClient : CodeActivity<FWBS.OMS.Client>
    {
        #region Arguments
        #endregion

        #region Constructor
        public FindClient()
        {
            this.DisplayName = "Find Client";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override FWBS.OMS.Client Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.Searches.FindClient();
        }
        #endregion
    }
}

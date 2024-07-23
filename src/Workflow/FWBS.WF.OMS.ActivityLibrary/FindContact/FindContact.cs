#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindContact.FindContactToolboxIcon.bmp")]
    [Description("Find a Contact")]
    [Designer(typeof(FindContactDesigner))]
    public sealed class FindContact : CodeActivity<FWBS.OMS.Contact>
    {
        #region Arguments
        #endregion

        #region Constructor
        public FindContact()
        {
            this.DisplayName = "Find Contact";
        }
        #endregion

        #region Overrides
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

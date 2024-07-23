#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindUser.FindUserToolboxIcon.png")]
    [Description("Find a User")]
    [Designer(typeof(FindUserDesigner))]
    public sealed class FindUser : CodeActivity<FWBS.OMS.User>
    {
        #region Arguments
        #endregion

        #region Constructor
        public FindUser()
        {
            this.DisplayName = "Find User";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override FWBS.OMS.User Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.Searches.FindUser();
        }
        #endregion
    }
}

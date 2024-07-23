#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindAddress.FindAddressToolboxIcon.png")]
    [Description("Find an Address")]
    [Designer(typeof(FindAddressDesigner))]
    public sealed class FindAddress : CodeActivity<FWBS.OMS.Address>
    {
        #region Arguments
        #endregion

        #region Constructor
        public FindAddress()
        {
            this.DisplayName = "Find Address";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override FWBS.OMS.Address Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.Searches.FindAddress();
        }
        #endregion
    }
}

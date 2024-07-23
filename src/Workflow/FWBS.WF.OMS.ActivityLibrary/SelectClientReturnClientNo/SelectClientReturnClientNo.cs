#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectClientReturnClientNo.SelectClientReturnClientNoToolboxIcon.bmp")]
    [Description("Returns a selected Client Number")]
    [Designer(typeof(SelectClientReturnClientNoDesigner))]
    public sealed class SelectClientReturnClientNo : CodeActivity<string>
    {
        #region Arguments
        #endregion

        #region Constructor
        public SelectClientReturnClientNo()
        {
            this.DisplayName = "Return Selected Client No";
        }
        #endregion

        #region Execute
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override string Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.SelectClientReturnClientNo();
        }
        #endregion
    }
}

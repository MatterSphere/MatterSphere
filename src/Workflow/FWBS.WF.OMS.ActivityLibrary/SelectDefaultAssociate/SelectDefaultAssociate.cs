#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SelectDefaultAssociate.SelectDefaultAssociateToolboxIcon.bmp")]
    [Description("Selects the default Associate")]
    [Designer(typeof(SelectDefaultAssociateDesigner))]
    public sealed class SelectDefaultAssociate : CodeActivity<FWBS.OMS.Associate>
    {
        #region Arguments
        #endregion

        #region Constructor
        public SelectDefaultAssociate()
        {
            this.DisplayName = "Select Default Associate";
        }
        #endregion

        #region Execute
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override FWBS.OMS.Associate Execute(CodeActivityContext context)
        {
            return FWBS.OMS.UI.Windows.Services.SelectDefaultAssociate();
        }
        #endregion
    }
}

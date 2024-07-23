#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreatePreClient.CreateCorporatePreClientToolboxIcon.bmp")]
    [Description("Creates a new Corporate PreClient")]
    [Designer(typeof(CreateCorporatePreClientDesigner))]
    public sealed class CreateCorporatePreClient : CodeActivity<bool>
    {
        #region Arguments
        #endregion

        #region Constructor
        public CreateCorporatePreClient()
        {
            this.DisplayName = "Create Corporate PreClient";
        }
        #endregion

        #region Execute
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool Execute(CodeActivityContext context)
        {
            object obj = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRCLIPRECRPNEW", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
            return obj != null;
        }
        #endregion
    }
}

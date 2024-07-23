#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreatePreClient.CreatePreClientToolboxIcon.bmp")]
    [Description("Creates a new PreClient")]
    [Designer(typeof(CreatePreClientDesigner))]
    public sealed class CreatePreClient : CodeActivity<bool>
    {
        #region Arguments
        #endregion

        #region Constructor
        public CreatePreClient()
        {
            this.DisplayName = "Create PreClient";
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
            object obj = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRCLIPRENEW", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
            return obj != null;
        }
        #endregion
    }
}

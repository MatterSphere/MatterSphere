#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowCurrentJobs.ShowCurrentJobsToolboxIcon.bmp")]
    [Description("Shows the current Job")]
    [Designer(typeof(ShowCurrentJobsDesigner))]
    public sealed class ShowCurrentJobs : CodeActivity
    {
        #region Constructor
        public ShowCurrentJobs()
        {
            this.DisplayName = "Show Current Jobs";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        protected override void Execute(CodeActivityContext context)
        {
            FWBS.OMS.UI.Windows.Services.ShowCurrentJobs();
        }
        #endregion
    }
}

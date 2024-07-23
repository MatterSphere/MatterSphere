#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowAssociate.ShowAssociateToolboxIcon.bmp")]
	[Description("Shows an Associate")]
	[Designer(typeof(ShowAssociateDesigner))]
	public sealed class ShowAssociate : CodeActivity
	{
		#region Constructor
		public ShowAssociate()
		{
			this.DisplayName = "Show Associate";
		}
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Associate> Associate { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Associate", typeof(FWBS.OMS.Associate), ArgumentDirection.In, true);
			metadata.Bind(this.Associate, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.Associate associate = Associate.Get(context);

			if (associate == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Associate' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ShowAssociate(associate);
			}
		}
		#endregion
	}
}

#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.RunPrecedent.RunPrecedentToolboxIcon.png")]
	[Description("Run a Precedent")]
	[Designer(typeof(RunPrecedentDesigner))]
	public sealed class RunPrecedent : CodeActivity<FWBS.OMS.PrecedentJob>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Precedent> Precedent { get; set; }

		[RequiredArgument]
		public InArgument<FWBS.OMS.Associate> Associate { get; set; }
		#endregion

		#region Constructor
		public RunPrecedent()
		{
			this.DisplayName = "Run Precedent";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Precedent", typeof(FWBS.OMS.Precedent), ArgumentDirection.In, true);
			metadata.Bind(this.Precedent, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Associate", typeof(FWBS.OMS.Associate), ArgumentDirection.In, true);
			metadata.Bind(this.Associate, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.PrecedentJob Execute(CodeActivityContext context)
		{
			FWBS.OMS.Precedent precedent = Precedent.Get(context);
			FWBS.OMS.Associate associate = Associate.Get(context);

			if (precedent == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.ControlApp' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.TemplateStart(null, precedent, associate);
			}
		}
		#endregion
	}
}

#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowOMSType.ShowOMSTypeToolboxIcon.png")]
	[Description("Shows a Type")]
	[Designer(typeof(ShowOMSTypeDesigner))]
	public sealed class ShowOMSType : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		[Description("Type Object to be displayed")]
		public InArgument<FWBS.OMS.Interfaces.IOMSType> Entity { get; set; }

		public InArgument<string> DefaultPage { get; set; }
		#endregion

		#region Constructor
		public ShowOMSType()
		{
			this.DisplayName = "Show OMS Type";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Entity", typeof(FWBS.OMS.Interfaces.IOMSType), ArgumentDirection.In, true);
			metadata.Bind(this.Entity, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("DefaultPage", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.DefaultPage, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			var omsObject = Entity.Get(context);
			var defaultPage = DefaultPage.Get(context);

			if (omsObject == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Entity' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ShowOMSType(omsObject, defaultPage);
			}
		}
		#endregion
	}
}

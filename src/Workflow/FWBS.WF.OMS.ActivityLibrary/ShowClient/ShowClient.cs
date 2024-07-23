#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowClient.ShowClientToolboxIcon.bmp")]
	[Description("Shows a Client")]
	[Designer(typeof(ShowClientDesigner))]
	public sealed class ShowClient : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Client> Client { get; set; }
		#endregion

		#region Constructor
		public ShowClient()
		{
			this.DisplayName = "Show Client";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Client", typeof(FWBS.OMS.Client), ArgumentDirection.In, true);
			metadata.Bind(this.Client, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.Client client = Client.Get(context);

			if (client == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Client' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ShowClient(client);
			}
		}
		#endregion
	}
}

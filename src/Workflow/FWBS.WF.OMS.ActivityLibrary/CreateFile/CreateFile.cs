#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateFile.CreateFileToolboxIcon.bmp")]
	[Description("Creates a new File")]
	[Designer(typeof(CreateFileDesigner))]
	public sealed class CreateFile : CodeActivity<FWBS.OMS.OMSFile>
	{
		#region Constructor
		public CreateFile()
		{
			this.DisplayName = "Create File";
		}
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Client> Client { get; set; }
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
        /// <returns></returns>
		protected override FWBS.OMS.OMSFile Execute(CodeActivityContext context)
		{
			FWBS.OMS.Client client = this.Client.Get(context);

			if (client == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Client' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(client);
			}
		}
		#endregion
	}
}

#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateContact.CreateContactToolboxIcon.bmp")]
	[Description("Creates a new Contact")]
	[Designer(typeof(CreateContactDesigner))]
	public sealed class CreateContact : CodeActivity<FWBS.OMS.Contact>
	{
		#region Constructor
		public CreateContact()
		{
			this.DisplayName = "Create Contact";
		}
		#endregion

		#region Arguments
		public InArgument<FWBS.OMS.Client> Client { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Client", typeof(FWBS.OMS.Client), ArgumentDirection.In);
			metadata.Bind(this.Client, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Contact Execute(CodeActivityContext context)
		{
			FWBS.OMS.Client client = this.Client.Get(context);

			if (client == null)
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateContact();
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateContact(client, true);
			}
		}
		#endregion
	}
}

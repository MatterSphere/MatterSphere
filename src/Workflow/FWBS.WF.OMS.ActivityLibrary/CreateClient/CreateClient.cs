#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateClient.CreateClientToolboxIcon.bmp")]
	[Description("Creates a new Client")]
	[Designer(typeof(CreateClientDesigner))]
	public sealed class CreateClient : CodeActivity<FWBS.OMS.Client>
	{
		#region Arguments

        public InArgument<FWBS.OMS.Contact> DefaultContact { get; set; }

		#endregion

		#region Constructor
		public CreateClient()
		{
			this.DisplayName = "Create Client";
		}
		#endregion

		#region Overrides


        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            var argument = new RuntimeArgument("DefaultContact", typeof(FWBS.OMS.Contact), ArgumentDirection.In, false);
            metadata.Bind(this.DefaultContact, argument);
            metadata.AddArgument(argument);
        }


        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Client Execute(CodeActivityContext context)
		{
            FWBS.OMS.Contact cont = this.DefaultContact.Get(context);

            return FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(cont, false);
		}
		#endregion
	}
}

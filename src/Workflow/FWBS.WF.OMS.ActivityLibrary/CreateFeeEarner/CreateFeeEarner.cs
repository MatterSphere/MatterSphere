#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateFeeEarner.CreateFeeEarnerToolboxIcon.png")]
	[Description("Creates a new Fee Earner")]
	[Designer(typeof(CreateFeeEarnerDesigner))]
	public sealed class CreateFeeEarner : CodeActivity<FWBS.OMS.FeeEarner>
	{
		#region Constructor
		public CreateFeeEarner()
		{
			this.DisplayName = "Create FeeEarner";
		}
		#endregion

		#region Arguments
		public InArgument<FWBS.OMS.User> User { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("User", typeof(FWBS.OMS.User), ArgumentDirection.In);
			metadata.Bind(this.User, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.FeeEarner Execute(CodeActivityContext context)
		{
			FWBS.OMS.User user = this.User.Get(context);

			if (user == null)
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateFeeEarner();
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateFeeEarner(user);
			}
		}
		#endregion
	}
}

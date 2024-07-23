#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateUser.CreateUserToolboxIcon.png")]
	[Description("Creates a new User")]
	[Designer(typeof(CreateUserDesigner))]
	public sealed class CreateUser : CodeActivity<FWBS.OMS.User>
	{
		#region Arguments
		#endregion

		#region Constructor
		public CreateUser()
		{
			this.DisplayName = "Create User";
		}
		#endregion

		#region Execute
		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override FWBS.OMS.User Execute(CodeActivityContext context)
		{
			return FWBS.OMS.UI.Windows.Services.Wizards.CreateUser();
		}
		#endregion
	}
}

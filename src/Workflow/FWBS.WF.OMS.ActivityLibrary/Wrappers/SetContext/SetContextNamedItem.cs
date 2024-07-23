#region References
using System;
using System.Activities;
using System.Activities.Expressions;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetContext.SetContextToolboxIcon.png")]
	[Description("Set Context")]
	[Designer(typeof(SetContextNamedItemDesigner))]
	public sealed class SetContextNamedItem : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Key { get; set; }
		[RequiredArgument]
		public InArgument<object> Value { get; set; }
		#endregion

		#region Constructor
		public SetContextNamedItem()
		{
			this.DisplayName = "Set Context Named Item";
		}

		#endregion

		#region Overrides
		//
		// IMPORTANT NOTE:
		//	Sync this execution code with the FWBS.WF.ActivityLibrary.SetContextNamedItem
		//	Literals cause problems and for a quick solution this activity implements the code below rather than
		//	scheduling FWBS.WF.ActivityLibrary.SetContextNamedItem.
		//
		protected override void Execute(CodeActivityContext context)
		{
			// get argument values and validate
			string key = this.Key.Get<string>(context);
			object obj = this.Value.Get<object>(context);

			// key must have a value!
			if (string.IsNullOrWhiteSpace(key))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Key' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}

			// get context
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();
			if (fwbsContext != null)
			{
				fwbsContext.Set(key, obj);
			}
		}
		#endregion

		#region Methods
		private Activity CreateImplementation()
		{
			return new FWBS.WF.ActivityLibrary.SetContextNamedItem
			{
				Key = new InArgument<string>(ctx => this.Key.Get(ctx)),
				Value = new ArgumentReference<Object>("Value"),
			};
		}
		#endregion
	}
}

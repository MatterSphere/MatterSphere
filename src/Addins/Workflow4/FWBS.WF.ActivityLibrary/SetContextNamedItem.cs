#region References
using System;
using System.Activities;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	public sealed class SetContextNamedItem : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Key { get; set; }
		[RequiredArgument]
		public InArgument<object> Value { get; set; }
		#endregion

		#region Overrides
		//
		// IMPORTANT NOTE:
		//	Sync this execution code with the FWBS.WF.OMS.ActivityLibrary.SetContextNamedItem
		//	Literals cause problems and for a quick solution FWBS.WF.OMS.ActivityLibrary.SetContextNamedItem implements the code below rather than
		//	scheduling this activity.
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
	}
}

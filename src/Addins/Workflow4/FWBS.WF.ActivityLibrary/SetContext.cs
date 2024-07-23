#region References
using System;
using System.Activities;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	public sealed class SetContext : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<object> Item { get; set; }
		#endregion

		#region Overrides
		protected override void Execute(CodeActivityContext context)
		{
			// get argument values and validate
			object obj = this.Item.Get<object>(context);

			// key must have a value!
			if (obj == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Item' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);			
			}

			// get context
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();
			if (fwbsContext != null)
			{
				fwbsContext.Set(obj);
			}
		}
		#endregion
	}
}

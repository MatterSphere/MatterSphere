#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region ClearDictionary class
	// The ClearDictionary will clear a dictionary.
	public sealed class ClearDictionary<TKey, TValue> : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<IDictionary<TKey, TValue>> Dictionary { get; set; }
		#endregion

		#region Overrides
		protected override void Execute(CodeActivityContext context)
		{
			IDictionary<TKey, TValue> dictionary = this.Dictionary.Get(context);
			if (dictionary == null)
			{
				throw new InvalidOperationException("The Dictionary has not been initialized");
			}

			dictionary.Clear();
		}
		#endregion
	}
	#endregion
}
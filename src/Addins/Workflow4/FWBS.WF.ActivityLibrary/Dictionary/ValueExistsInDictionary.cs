#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region ValueExistsInDictionary class
	// The ValueExistsInDictionary activity determines if a value exists in a dictionary
	public sealed class ValueExistsInDictionary<TKey, TValue> : CodeActivity<bool>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<IDictionary<TKey, TValue>> Dictionary { get; set; }
		[RequiredArgument]
		public InArgument<TValue> Value { get; set; }
		#endregion

		#region Overrides
		protected override bool Execute(CodeActivityContext context)
		{
			IDictionary<TKey, TValue> dictionary = this.Dictionary.Get(context);

			if (dictionary == null)
			{
				throw new InvalidOperationException("The Dictionary has not been initialized");
			}

			return dictionary.Values.Contains(this.Value.Get(context));
		}
		#endregion
	}
	#endregion
}
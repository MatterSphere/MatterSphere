#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region GetFromDictionary class
	// The GetFromDictionary activity will get the value of the key from a dictionary.
	public sealed class GetFromDictionary<TKey, TValue> : CodeActivity<bool>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<IDictionary<TKey, TValue>> Dictionary { get; set; }
		[RequiredArgument]
		public InArgument<TKey> Key { get; set; }
		public OutArgument<TValue> Value { get; set; }
		#endregion

		#region Overrides
		protected override bool Execute(CodeActivityContext context)
		{
			IDictionary<TKey, TValue> dictionary = this.Dictionary.Get(context);
			if (dictionary == null)
			{
				throw new InvalidOperationException("The Dictionary has not been initialized");
			}

			TKey key = this.Key.Get(context);
			TValue value;
			bool result = dictionary.TryGetValue(key, out value);
			this.Value.Set(context, value);

			return result;
		}
		#endregion
	}
	#endregion
}
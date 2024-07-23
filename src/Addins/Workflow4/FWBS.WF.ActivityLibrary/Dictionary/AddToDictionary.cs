#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region AddToDictionary class
	// The AddToDictionary activity will add a key value pair to a dictionary.
	public sealed class AddToDictionary<TKey, TValue> : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<IDictionary<TKey, TValue>> Dictionary { get; set; }
		[RequiredArgument]
		public InArgument<TKey> Key { get; set; }
		public InArgument<TValue> Value { get; set; }
		#endregion

		#region Overrides
		protected override void Execute(CodeActivityContext context)
		{
			IDictionary<TKey, TValue> dictionary = this.Dictionary.Get(context);
			if (dictionary == null)
			{
				throw new InvalidOperationException("The Dictionary has not been initialized");
			}

			TKey key = this.Key.Get(context);
			dictionary.Add(key, this.Value.Get(context));
		}
		#endregion
	}
	#endregion
}
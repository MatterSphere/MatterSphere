﻿#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region RemoveFromDictionary class
	// The RemoveFromDictionary activity will remove a key value pair to a dictionary.
	public sealed class RemoveFromDictionary<TKey, TValue> : CodeActivity<bool>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<IDictionary<TKey, TValue>> Dictionary { get; set; }
		[RequiredArgument]
		public InArgument<TKey> Key { get; set; }
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
			return dictionary.Remove(key);
		}

		#endregion
	}
	#endregion
}
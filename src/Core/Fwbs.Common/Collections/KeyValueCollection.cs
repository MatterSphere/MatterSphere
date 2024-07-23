using System;

namespace FWBS.Common.Collections
{
	/// <summary>
	/// Value key pair object item.
	/// </summary>
	public class KeyValueItem
	{
		private object _value;
		private string _key;

		public KeyValueItem(string key, object value)
		{
			_value = value;
			_key = key;
		}

		public string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public override string ToString()
		{
			return Convert.ToString(Key + " - " + Convert.ToString(Value));
		}
	}

	/// <summary>
	/// A collection that is the same as the built in System.Collections.Specialized.NameValueCollection
	/// but uses an object as the stored data.
	/// </summary>
	public class KeyValueCollection :  System.Collections.Specialized.NameObjectCollectionBase
	{
		public void Add(KeyValueItem value)
		{
			base.BaseAdd(value.Key, value.Value);
		}

		public void Add(string key, object value)
		{
			base.BaseAdd(key, value);
		}

		public void Remove(string key)
		{
			base.BaseRemove(key);
		}

		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		public void Clear()
		{
			base.BaseClear();
		}

		public bool Contains(string key)
		{
			return (this[key] != null);
		}

		/// <summary>
		/// Gets the item in the collection by its key value.
		/// </summary>
		public KeyValueItem this[string key]
		{
			get
			{
				object val = base.BaseGet(key);
				if (val == null)
					return null;
				else
					return new KeyValueItem(key, val);
			}
		}

		/// <summary>
		/// Gets the item in the collection by its index value.
		/// </summary>
		public KeyValueItem this[int index]
		{
			get
			{
				object val = base.BaseGet(index);
				if (val == null)
					return null;
				else
				{
					string key = base.BaseGetKey(index);
					return new KeyValueItem(key, val);
				}
			}
		}

	}
}

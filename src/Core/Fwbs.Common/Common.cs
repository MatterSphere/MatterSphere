using System;
using System.Data;
using System.Runtime.Serialization;

namespace FWBS.Common
{
	/// <summary>
	/// Holds information on a single enumeration member item.
	/// </summary>
	public struct EnumListItem
	{
		private System.Int64 _value;
		private string _name;

		public EnumListItem (System.Int64 value, string name)
		{
			_value = value;
			_name = name;
		}

		public System.Int64 Value
		{
			get
			{
				return _value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

        /// <summary>
        /// Converts an enumeration type name into an array list so that it can be
        /// displayed straight into a list containung control.
        /// </summary>
        /// <param name="enumerationname">Enumeration name to list.</param>
        /// <returns>An array list.</returns>
        public static System.Collections.ArrayList EnumToList (string enumerationname)
		{
			return EnumToList(Type.GetType(enumerationname));
		}


		/// <summary>
		/// Converts an enumeration type into an array list so that it can be
		/// displayed straight into a list containung control.
		/// </summary>
		/// <param name="enumeration">Enumeration type to list.</param>
		/// <returns>An array list.</returns>
		public static System.Collections.ArrayList EnumToList (Type enumeration)
		{
			Array arr = Enum.GetValues(enumeration);
			System.Collections.ArrayList list = new System.Collections.ArrayList(arr.Length);
					
			for (int ctr = 0; ctr < arr.Length; ctr++)
			{
				list.Add(new EnumListItem(Convert.ToInt64(arr.GetValue(ctr)), Enum.GetName(enumeration, arr.GetValue(ctr))));
			}
			return list;
		}
	}


	/// <summary>
	/// Holds information on a string enumeration member item.
	/// </summary>
	public struct StringListItem
	{
		private string _value;
		private string _name;

		public StringListItem (string value, string name)
		{
			_value = value;
			_name = name;
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

        /// <summary>
        /// Converts an Comma Seperated List into an array list so that it can be
        /// displayed straight into a list containung control.
        /// </summary>
        /// <param name="input">Enumeration type to list.</param>
        /// <returns>An array list.</returns>
        public static System.Collections.ArrayList CommaStringToList (string input)
		{
			Array arr = input.Split(",".ToCharArray());
			System.Collections.ArrayList list = new System.Collections.ArrayList(arr.Length);
			try
			{
				for (int ctr = 0; ctr < arr.Length; ctr = ctr + 2)
				{
					list.Add(new StringListItem(Convert.ToString(arr.GetValue(ctr)), Convert.ToString(arr.GetValue(ctr+1))));
				}
			}
			catch
			{}
			return list;
		}
	}
		

	public class ConvertDef
	{

        /// <summary>
        /// Converts the specified value to a DateTime, using the default value if it cannot be converted.
        /// </summary>
        /// <param name="value">The value to convert to a DateTime.</param>
        /// <param name="def">The default value to use if the the value cannot be converted.</param>
        /// <returns>A DateTime value whether it is the value passed or the default value specified.</returns>
        public static System.DateTime ToDateTime(object value, System.DateTime def)
        {
            try
            {
                string str = value as string;

                if (str == null)
                {
                    if (value == DBNull.Value || value == null)
                        return def;
                    else
                        return Convert.ToDateTime(value, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    System.DateTime ret;
                    if (DateTime.TryParse(str, out ret))
                        return ret;
                    else
                        return def;
                }
            }
            catch (ArgumentException)
            {
                return def;
            }
            catch (InvalidCastException)
            {
                return def;
            }
        }

		public static DateTimeNULL ToDateTimeNULL(object value, DateTimeNULL def)
		{
			try
			{
				if (value is DBNull)
					return DBNull.Value;
                else if (value is DateTime)
                    return (DateTime)value;
                else if (value is DateTimeNULL)
                    return (DateTimeNULL)value;
                else if (value is string)
                    return (string)value;
                else
                    return DBNull.Value;
			}
			catch
			{
				return def;
			}

		}

		public static object ToEnum(object value,object retdef)
		{
			try
			{
				return Enum.Parse(retdef.GetType(),Convert.ToString(value),true);
			}
			catch
			{
				return retdef;
			}
		}

		public static bool ToBoolean(object value,bool retdef)
		{
			try
			{
				return Convert.ToBoolean(value);
			}
			catch
			{
				return retdef;
			}
		}

        public static Guid ToGuid(object value, Guid retdef)
        {
            Guid guid;
            if (Guid.TryParse(value.ToString(), out guid))
            {
                return guid;
            }

            return retdef;
        }

        public static Int32 ToInt32(object value, Int32 retdef)
		{
			try
			{
				return Convert.ToInt32(value);
			}
			catch
			{
				return retdef;
			}
		}

		public static Double ToDouble(object value, Double retdef)
		{
			try
			{
				return Convert.ToDouble(value);
			}
			catch
			{
				return retdef;
			}
		}

		public static Int64 ToInt64(object value, Int64 retdef)
		{
			try
			{
				return Convert.ToInt64(value);
			}
			catch
			{
				return retdef;
			}
		}

		public static Int16 ToInt16(object value, Int16 retdef)
		{
			try
			{
				return Convert.ToInt16(value);
			}
			catch
			{
				return retdef;
			}
		}
		public static Decimal ToDecimal(object value, Decimal retdef)
		{
			try
			{
				return Convert.ToDecimal(value);
			}
			catch
			{
				return retdef;
			}
		}

        public static byte ToByte(object value, byte retdef)
        {
            try
            {
                return Convert.ToByte(value);
            }
            catch
            {
                return retdef;
            }
        }
    }

	
	public sealed class Format
	{
		private Format(){}

		public static string GetFormattedValue(object val, string format)
		{
			string newval = "";
			
			if (val != null)
			{
				newval = val.ToString();
				if (val is DateTime)
				{
					newval = ((DateTime)val).ToString(format);
				}
				else if (val is Int16)
				{
					newval = ((Int16)val).ToString(format);
				}
				else if (val is Int32)
				{
					newval = ((Int32)val).ToString(format);
				}
				else if (val is Int64)
				{
					newval = ((Int64)val).ToString(format);
				}
				else if ( val is Common.DateTimeNULL)
				{
					Common.DateTimeNULL dte = (Common.DateTimeNULL)val;
					if (dte.IsNull)
						newval = "";
					else
						newval = ((DateTime)dte).ToString(format);

				}
			}
			return newval;
		}
	}

	public class FormPercentageSize
	{
		public static System.Drawing.Size PercentageOfMainScreen(int PercentageWidth, int PercentageHeight)
		{
			// 1024 x 768
			// 80% x 80%
			int Width = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width * PercentageWidth) / 100;
			int Height = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * PercentageHeight) / 100;
			return new System.Drawing.Size(Width,Height);
		}

		public static System.Drawing.Size PercentageOfScreenFromHandle(System.IntPtr Handle, int PercentageWidth, int PercentageHeight)
		{
			// 1024 x 768
			// 80% x 80%
			int Width = (System.Windows.Forms.Screen.FromHandle(Handle).WorkingArea.Width * PercentageWidth) / 100;
			int Height = (System.Windows.Forms.Screen.FromHandle(Handle).WorkingArea.Height * PercentageHeight) / 100;
			return new System.Drawing.Size(Width,Height);
		}
	}
	
	public class OMSDebug
	{
		#region Debug
		public static string DataTableToString(DataTable dt)
		{
			string cr = Environment.NewLine;
			System.Text.StringBuilder strbuilder = new System.Text.StringBuilder();
			strbuilder.Append(cr);
			strbuilder.Append("Table Name : " + dt.TableName + cr);
			strbuilder.Append(cr);
			int maxwidth = 0;
			foreach (DataColumn cm in dt.Columns)
			{
				int ml = cm.MaxLength;
				if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
				if (ml < 15) ml = 15;
				if (ml > 150) ml = 150;
				strbuilder.Append(cm.ColumnName.PadRight(ml).Substring(0,ml) + " ");
				maxwidth += ml+ 1;
			}
			strbuilder.Append(cr);
			strbuilder.Append("".PadRight(maxwidth,'_') + cr);
			foreach (DataRow rw in dt.Rows)
			{
				foreach (DataColumn cm in dt.Columns)
				{
					int ml = cm.MaxLength;
					if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
					if (ml < 15) ml = 15;
					if (ml > 150) ml = 150;
					strbuilder.Append(Convert.ToString(rw[cm.ColumnName]).PadRight(ml).Substring(0,ml) + " ");
				}
				strbuilder.Append(cr);
			}
			strbuilder.Append(cr);
			return strbuilder.ToString();
		}
		
		public static bool DebugDataViewList(DataView dt)
		{
			string cr = Environment.NewLine;
			System.Text.StringBuilder strbuilder = new System.Text.StringBuilder();
			strbuilder.Append(cr);
			strbuilder.Append("Table Name : " + dt.Table.TableName + cr);
			strbuilder.Append(cr);
			int maxwidth = 0;
			foreach (DataColumn cm in dt.Table.Columns)
			{
				int ml = cm.MaxLength;
				if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
				if (ml < 15) ml = 15;
				if (ml > 150) ml = 150;
				strbuilder.Append(cm.ColumnName.PadRight(ml).Substring(0,ml) + " ");
				maxwidth += ml+ 1;
			}
			strbuilder.Append(cr);
			strbuilder.Append("".PadRight(maxwidth,'-') + cr);
			foreach (DataRowView rw in dt)
			{
				foreach (DataColumn cm in dt.Table.Columns)
				{
					int ml = cm.MaxLength;
					if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
					if (ml < 15) ml = 15;
					if (ml > 150) ml = 150;
					strbuilder.Append(Convert.ToString(rw[cm.ColumnName]).PadRight(ml).Substring(0,ml) + " ");
				}
				strbuilder.Append(cr);
			}
			strbuilder.Append(cr);
			System.Diagnostics.Trace.Write(strbuilder.ToString());
			return true;
		}
		
		public static bool DebugDataTableList(DataTable dt)
		{
			string cr = Environment.NewLine;
			System.Text.StringBuilder strbuilder = new System.Text.StringBuilder();
			strbuilder.Append(cr);
			strbuilder.Append("Table Name : " + dt.TableName + cr);
			strbuilder.Append(cr);
			int maxwidth = 0;
			foreach (DataColumn cm in dt.Columns)
			{
				int ml = cm.MaxLength;
				if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
				if (ml < 15) ml = 15;
				if (ml > 150) ml = 150;
				strbuilder.Append(cm.ColumnName.PadRight(ml).Substring(0,ml) + " ");
				maxwidth += ml+ 1;
			}
			strbuilder.Append(cr);
			strbuilder.Append("".PadRight(maxwidth,'-') + cr);
			foreach (DataRow rw in dt.Rows)
			{
				foreach (DataColumn cm in dt.Columns)
				{
					int ml = cm.MaxLength;
					if (ml < cm.ColumnName.Length) ml = cm.ColumnName.Length;
					if (ml < 15) ml = 15;
					if (ml > 150) ml = 150;
					strbuilder.Append(Convert.ToString(rw[cm.ColumnName]).PadRight(ml).Substring(0,ml) + " ");
				}
				strbuilder.Append(cr);
			}
			strbuilder.Append(cr);
			System.Diagnostics.Trace.Write(strbuilder.ToString());
			return true;
		}
		#endregion
	}

	/// <summary>
	/// Value key pair object item.
	/// </summary>
    [Serializable]
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
    [Serializable]
    public class KeyValueCollection :  System.Collections.Specialized.NameObjectCollectionBase
	{
        public KeyValueCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public KeyValueCollection() : base()
        {

        }

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

    /// <summary>
    /// A collection which is used to maximise the number of items within the collection.
    /// All those items that have not been used for a while will be pushed out of the collection.
    /// </summary>
    public class LimitCollection : System.Collections.Specialized.NameObjectCollectionBase, IDisposable
    {
        private sealed class CachedObject
        {
            public CachedObject(string key, object data, DateTime cached)
            {
                this.key = key;
                this.data = data;
                this.cached = cached;
            }

            private readonly string key;
            public string Key
            {
                get
                {
                    return key;
                }
            }

            private readonly DateTime cached;
            public DateTime Timeout
            {
                get
                {
                    return cached;
                }
            }

            private readonly Object data;
            public object Data
            {
                get
                {
                    return data;
                }
            }

        }

        private readonly object synch = new object();

        /// <summary>
        /// Holds the maximum number of items.
        /// </summary>
        private int _max = 5;

        /// <summary>
        /// Holds the number of minutes a item will be cached for.
        /// </summary>
        private int _minutes = 10;

        /// <summary>
        /// A constructor which specifies the maximum number of items allowed in the collection.
        /// </summary>
        /// <param name="max"></param>
        public LimitCollection(int max)
        {
            _max = max;
        }

        /// <summary>
        /// A constructor which specifies the maximum number of items allowed and the amount of time to be cached.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="minutes"></param>
        public LimitCollection(int max, int minutes)
        {
            _max = max;
            _minutes = minutes;
        }

        /// <summary>
        /// Adds an item to the collection. 
        /// </summary>
        /// <param name="key">A unique value to identfy the object with.</param>
        /// <param name="val">The actual value to store.</param>
        public void Add(string key, object val)
        {
            lock (synch)
            {
                CachedObject co = (CachedObject)BaseGet(key);
                DateTime? timeout = null;
                if (co != null)
                {
                    timeout = co.Timeout;
                    BaseRemove(key);
                }

                if (Count >= _max)
                    BaseRemoveAt(0);

                co = new CachedObject(key, val, (timeout ?? DateTime.UtcNow.AddMinutes(_minutes)));

                BaseAdd(key, co);
            }
        }


        /// <summary>
        /// Gets the item in the collection by its key value.
        /// </summary>
        public object this[string key]
        {
            get
            {
                lock (synch)
                {
                    CachedObject co = (CachedObject)BaseGet(key);
                    if (co == null || co.Data == null)
                    {
                        return null;
                    }
                    else
                    {
                        if (co.Timeout <= DateTime.UtcNow && _minutes >= 0)
                        {
                            BaseRemove(co.Key);
                            return null;
                        }

                        Add(co.Key, co.Data);
                        return co.Data;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the item in the collection by its index.
        /// </summary>
        public object this[int idx]
        {
            get
            {
                lock (synch)
                {
                    CachedObject co = (CachedObject)BaseGet(idx);
                    if (co == null || co.Data == null)
                    {
                        return null;
                    }
                    else
                    {
                        if (co.Timeout <= DateTime.UtcNow && _minutes >= 0)
                        {
                            BaseRemove(co.Key);
                            return null;
                        }

                        Add(co.Key, co.Data);
                        return co.Data;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the last item added to the collection.
        /// </summary>
        public object LastItem
        {
            get
            {
                if (Count > 0)
                    return this[Count - 1];
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets all of the values.
        /// </summary>
        public object[] Values
        {
            get
            {
                lock (synch)
                {
                    object[] vals = BaseGetAllValues();
                    System.Collections.ArrayList list = new System.Collections.ArrayList();
                    foreach (CachedObject co in vals)
                    {
                        if (co.Timeout > DateTime.UtcNow || _minutes < 0)
                            list.Add(co.Data);
                    }

                    return list.ToArray();
                }
            }
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Clears the specified item from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Checks if a valid key item already exists within the collection.
        /// </summary>
        /// <param name="key">The key to test for.</param>
        /// <returns>True or false, depending on existence.</returns>
        public bool Contains(string key)
        {
            CachedObject co = (CachedObject)BaseGet(key);
            if (co == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Disposes of the collection.
        /// </summary>
        public void Dispose()
        {
            BaseClear();
        }

    }


	

	/// <summary>
	/// Class of OMS mathematical functions not covered by .NET framework
	/// </summary>
	public sealed class Math
	{
		/// <summary>
		/// Rounds up a decimal number to 2 decimal places for formatting in controls
		///	NOTE: The standard .net Round method rounds down
		/// </summary>
		/// <param name="val">Value to round E.G. 3.555 will round to 3.56</param>
		/// <returns>Number rounded to 2 decimal places rounding up if .005</returns>
		public static decimal RoundUp(decimal val)
		{
			//first multiply by 100 to get the first 2 existing decimal places to the left of the decimal point
			decimal dval = 	val * (decimal)100;
			
			//add .5 so that if the first decimal place is .5 or above it will round up
			dval = decimal.Add(dval,(decimal)0.5);
			
			//strip off number after decimal point
			double d = System.Math.Floor((double)dval);	
			
			//divide back by 100 to return to starting figure minus the trailing decimal places
			decimal retval = (decimal)d / 100;
			
			return retval;
		}

		public static int AgeFromDOB(int Year, int Month, int Day)
		{
			int tyear = DateTime.Today.Year - Year;
			int tmonth = DateTime.Today.Month;
			int tday = DateTime.Today.Day;

			if (Month < tmonth)
				return tyear;
			else if (tmonth == Month && tday < Day)
				return tyear - 1;
			else if (tmonth == Month && tday == Day)
				return tyear;
			else
				return tyear - 1;
		}
	}


}

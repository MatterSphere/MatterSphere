using System;

namespace FWBS.Common
{
    /// <summary>
    /// Conversion Class 
    /// </summary>
	public sealed class ConvDef
	{
		private ConvDef(){}

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

		public static Single ToSingle(object value, Single retdef)
		{
			try
			{
				return Convert.ToSingle(value);
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
	}
}

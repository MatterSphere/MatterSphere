using System;

namespace FWBS.Common.Data
{
	/// <summary>
	/// SQL specific routines.
	/// </summary>
	sealed public class SQLRoutines
	{
		/// <summary>
		/// Disables the creation of this class.
		/// </summary>
		private SQLRoutines()
		{
		}

		public static string ValidateSQL (string sql)
		{
			return sql.Replace("'", "''");
		}

        public static string ConvertToLikeValue(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            value = ValidateSQL(value);
            value = value.Replace("[", "ÿþýÊ");
            value = value.Replace("]", "Êÿýþ");
            value = value.Replace("ÿþýÊ", "[[]");
            value = value.Replace("Êÿýþ", "[]]");
            value = value.Replace("%", "[%]");

            return value;
        }

		/// <summary>
		/// Returns true if the object passed is a null, DBNull or String.Empty.
		/// </summary>
		/// <param name="val">Value to convert.</param>
		/// <returns>True if null string.</returns>
		public static bool IsNullString(object val)
		{
			if (val == null)
				return true;
			else if (val == DBNull.Value)
				return true;
			else if (val.ToString() == string.Empty)
				return true;
			else
				return false;
		}
	}
}

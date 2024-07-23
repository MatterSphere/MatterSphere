using System;


namespace FWBS.Common
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

		public static string RemoveRubbish (string sql)
		{
			return Data.SQLRoutines.ValidateSQL(sql);
		}

        public static string ConvertToLikeValue(string value)
        {
            return Data.SQLRoutines.ConvertToLikeValue(value);
        }

		/// <summary>
		/// Returns true if the object passed is a null, DBNull or String.Empty.
		/// </summary>
		/// <param name="val">Value to convert.</param>
		/// <returns>True if null string.</returns>
		public static bool IsNullString(object val)
		{
            return Data.SQLRoutines.IsNullString(val);
		}
	}
}

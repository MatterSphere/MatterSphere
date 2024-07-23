using System;

namespace FWBS.Common
{
    /// <summary>
    /// DateTimeNULL is an object that will allow DateTime but also NULL to be used.
    /// </summary>
	public struct DateTimeNULL : IFormattable, IComparable, IConvertible
	{
		#region Fields

		/// <summary>
		/// A flag that indicates that the current date in a Null.
		/// </summary>
		private bool _isNull;
		/// <summary>
		/// A date time field that holds the valid date when not Null.
		/// </summary>
		private DateTime _date;

		#endregion

		#region Operators

		#region Implicit Conversions

		/// <summary>
		/// Allows a null to be assigned to the date time value type directly.
		/// </summary>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>DateTimeNULL Value.</returns>
		public static implicit operator DateTimeNULL (DBNull Null)
		{
			DateTimeNULL date = new DateTimeNULL();
			date._isNull = true;
			return date;
		}

		/// <summary>
		/// Allows a DateTime value to be assigned to the date time value type directly.
		/// </summary>
		/// <param name="dte">A date time value.</param>
		/// <returns>DateTimeNULL Value.</returns>
		public static implicit operator DateTimeNULL (DateTime dte)
		{
			DateTimeNULL date = new DateTimeNULL();
			date._isNull = false;
            if (dte.Kind == DateTimeKind.Unspecified)
                date._date = DateTime.SpecifyKind(dte, DateTimeKind.Local);
            else
			    date._date = dte; 
			return date;
		}


		/// <summary>
		/// Converts a DateTimeNULL value directly into a DBNull.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <returns>If Null then a DBNull will be returned, otherwise a conversion exception will occur.</returns>
		public static implicit operator DBNull (DateTimeNULL dte)
		{
			if (dte._isNull)
				return DBNull.Value;
			else 
				return (DBNull)(Object)dte._date;
		}

		/// <summary>
		/// Converts a DateTimeNULL value directly into a DateTime.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <returns>If a valid date will be returned, otherwise a conversion exception will occur.</returns>
		public static implicit operator DateTime (DateTimeNULL dte)
		{
			if (dte._isNull)
				return (DateTime)(Object)DBNull.Value;
			else 
				return dte._date;
		}

		/// <summary>
		/// Converts a DateTimeNULL value directly into a string.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <returns>A string representation of the date, empty string if Null.</returns>
		public static implicit operator string (DateTimeNULL dte)
		{
			return dte.ToString();
		}

		/// <summary>
		/// Allows a valid date strinf to be parsed into a date or a Null if empty string.
		/// </summary>
		/// <param name="s">A string value representing a date.</param>
		/// <returns>DateTimeNULL Value.</returns>
		public static implicit operator DateTimeNULL (string s)
		{
			return DateTimeNULL.Parse(s);
		}

		#endregion

		#region == 

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if both values are equal to DBNull.Value.</returns>
		public static bool operator == (DateTimeNULL dte, DBNull Null)
		{
			if (dte.IsNull)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if both values are equal to DBNull.Value.</returns>
		public static bool operator == (DBNull Null, DateTimeNULL dte)
		{
			return (dte == Null);
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if both values are equal to the DateTime value.</returns>
		public static bool operator == (DateTimeNULL dte, DateTime dte2)
		{
            if (dte.IsNull)
                return false;
            else
            {
                if (dte._date.Kind == DateTimeKind.Unspecified)
                    dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

                if (dte2.Kind == DateTimeKind.Unspecified)
                    dte2 = DateTime.SpecifyKind(dte2, DateTimeKind.Local);

                return (dte._date.ToLocalTime() == dte2.ToLocalTime());
            }
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if both values are equal to the DateTime value.</returns>
		public static bool operator == (DateTime dte2, DateTimeNULL dte)
		{
			return (dte == dte2);
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if both values are equal.</returns>
		public static bool operator == (DateTimeNULL dte, DateTimeNULL dte2)
		{
			if (dte.IsNull == true && dte2.IsNull == true)
				return true;
			else 
			{
                if (dte.IsNull == false && dte2.IsNull == false)
                {
                    if (dte._date.Kind == DateTimeKind.Unspecified)
                        dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

                    if (dte2._date.Kind == DateTimeKind.Unspecified)
                        dte2._date = DateTime.SpecifyKind(dte2._date, DateTimeKind.Local);

                    return dte._date.ToLocalTime() == dte2._date.ToLocalTime();
                }
                else
                    return false;
			}
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if both values are equal to the string value.</returns>
		public static bool operator == (DateTimeNULL dte, string s)
		{
			DateTimeNULL d = s;
			return (dte == d);
		}

		/// <summary>
		/// Overloads the EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if both values are equal to the string value.</returns>
		public static bool operator == (string s, DateTimeNULL dte)
		{
			return (dte == s);
		}

		#endregion

		#region !=

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if both values are equal to DBNull.Value.</returns>
		public static bool operator != (DateTimeNULL dte, DBNull Null)
		{
			return (!(dte == Null));
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if both values are equal to DBNull.Value.</returns>
		public static bool operator != (DBNull Null, DateTimeNULL dte)
		{
			return (dte != Null);
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if both values are equal to the DateTime value.</returns>
		public static bool operator != (DateTimeNULL dte, DateTime dte2)
		{
			return (!(dte == dte2));
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if both values are equal to the DateTime value.</returns>
		public static bool operator != (DateTime dte2, DateTimeNULL dte)
		{
			return (dte != dte2);
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if both values are equal.</returns>
		public static bool operator != (DateTimeNULL dte, DateTimeNULL dte2)
		{
			return (!(dte == dte2));
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if both values are equal to the string value.</returns>
		public static bool operator != (DateTimeNULL dte, string s)
		{
			return (!(dte == s));
		}

		/// <summary>
		/// Overloads the Not EqualTo Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if both values are equal to the string value.</returns>
		public static bool operator != (string s, DateTimeNULL dte)
		{
			return (dte != s);
		}

		#endregion

		#region >

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DateTimeNULL value is greater than DBNull.Value.</returns>
		public static bool operator > (DateTimeNULL dte, DBNull Null)
		{
			if (dte == Null)
				return false;

			if (dte.IsNull)
				return false;
			else
				return true;
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DBNull.Value is greater than DateTimeNULL.</returns>
		public static bool operator > (DBNull Null, DateTimeNULL dte)
		{
			return false;
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTimeNULL value is greater than the DateTime value.</returns>
		public static bool operator > (DateTimeNULL dte, DateTime dte2)
		{
			if (dte == dte2)
				return false;

            if (dte.IsNull)
                return false;
            else
            {
                if (dte._date.Kind == DateTimeKind.Unspecified)
                    dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

                if (dte2.Kind == DateTimeKind.Unspecified)
                    dte2 = DateTime.SpecifyKind(dte2, DateTimeKind.Local);

                return (dte._date.ToLocalTime() > dte2.ToLocalTime());
            }
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTime value is greater than DateTimeNULL.</returns>
		public static bool operator > ( DateTime dte2, DateTimeNULL dte)
		{
			if (dte2 == dte)
				return false;

            if (dte.IsNull)
                return true;
            else
            {

                return (dte2 > dte._date);
            }
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if the DateTimeNULL value is greater than the other DateTimeNULL value.</returns>
		public static bool operator > (DateTimeNULL dte, DateTimeNULL dte2)
		{
			if (dte == dte2)
				return false;

			if (dte.IsNull && dte2.IsNull == false)
				return false;
			
			if (dte.IsNull == false && dte2.IsNull)
				return true;

            if (dte._date.Kind == DateTimeKind.Unspecified)
                dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

            if (dte2._date.Kind == DateTimeKind.Unspecified)
                dte2._date = DateTime.SpecifyKind(dte2._date, DateTimeKind.Local);


			return (dte._date > dte2._date);
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the DateTimeNULL value is greater than the string value.</returns>
		public static bool operator > (DateTimeNULL dte, string s)
		{
			DateTimeNULL d = s;
			return (dte > d);
		}

		/// <summary>
		/// Overloads the Greater Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the string value is greater than DateTimeNULL.</returns>
		public static bool operator > ( string s, DateTimeNULL dte)
		{
			DateTimeNULL d = s;
			return (d > dte);
		}

		#endregion

		#region <

		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DateTimeNULL value is less than DBNull.Value.</returns>
		public static bool operator < (DateTimeNULL dte, DBNull Null)
		{
			return false;
		}
		
		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DBNull.Value is less than DateTimeNULL.</returns>
		public static bool operator < (DBNull Null, DateTimeNULL dte)
		{
			if (dte == Null)
				return false;

			if (dte.IsNull)
				return false;
			else
				return true;
		}
		
		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTimeNULL value is less than the DateTime value.</returns>
		public static bool operator < (DateTimeNULL dte, DateTime dte2)
		{
			if (dte == dte2)
				return false;

            if (dte.IsNull)
                return true;
            else
            {
                if (dte._date.Kind == DateTimeKind.Unspecified)
                    dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

                if (dte2.Kind == DateTimeKind.Unspecified)
                    dte2 = DateTime.SpecifyKind(dte2, DateTimeKind.Local);


                return (dte._date < dte2);
            }
		}

		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTime value is less than DateTimeNULL.</returns>
		public static bool operator < (DateTime dte2, DateTimeNULL dte)
		{
			if (dte2 == dte)
				return false;

			if (dte.IsNull)
				return false;
			else
				return (dte2 < dte._date);
		}

		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if the DateTimeNULL value is less than the other DateTimeNULL value.</returns>
		public static bool operator < (DateTimeNULL dte, DateTimeNULL dte2)
		{
			if (dte == dte2)
				return false;

			if (dte.IsNull && dte2.IsNull == false)
				return true;
			
			if (dte.IsNull == false && dte2.IsNull)
				return false;

            if (dte._date.Kind == DateTimeKind.Unspecified)
                dte._date = DateTime.SpecifyKind(dte._date, DateTimeKind.Local);

            if (dte2._date.Kind == DateTimeKind.Unspecified)
                dte2._date = DateTime.SpecifyKind(dte2._date, DateTimeKind.Local);

            return (dte._date.ToLocalTime() < dte2._date.ToLocalTime());
		}

		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the DateTimeNULL value is less than the string value.</returns>
		public static bool operator < (DateTimeNULL dte, string s)
		{
			DateTimeNULL d = s;
			return (dte < d);
		}

		/// <summary>
		/// Overloads the Less Than Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the string value is less than DateTimeNULL.</returns>
		public static bool operator < (string s, DateTimeNULL dte)
		{
			DateTimeNULL d = s;
			return (d < dte);
		}

		#endregion

		#region >=

		/// <summary>
		/// Overloads the Greater Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DateTimeNULL value is greater than or equal to DBNull.Value.</returns>
		public static bool operator >= (DateTimeNULL dte, DBNull Null)
		{
			if ((dte > Null) || (dte == Null))
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// Overloads the Greater Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DBNull.Value is greater than or equal to DateTimeNULL.</returns>
		public static bool operator >= (DBNull Null, DateTimeNULL dte)
		{
			if ((Null > dte) || (Null == dte))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Greater Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTimeNULL value is greater than or equal to the DateTime value.</returns>
		public static bool operator >= (DateTimeNULL dte, DateTime dte2)
		{
			if ((dte > dte2) || (dte == dte2))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Greater Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTime value is greater than or equal to DateTimeNULL.</returns>
		public static bool operator >= (DateTime dte2, DateTimeNULL dte)
		{
			if ((dte2 > dte) || (dte2 == dte))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Greater Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if the DateTimeNULL value is greater than or equal to the other DateTimeNULL value.</returns>
		public static bool operator >= (DateTimeNULL dte, DateTimeNULL dte2)
		{
			if ((dte > dte2) || (dte == dte2))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Greater Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the DateTimeNULL value is greater than or equal to the string value.</returns>
		public static bool operator >= (DateTimeNULL dte, string s)
		{
			if ((dte > s) || (dte == s))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Greater Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the string value is greater than or equal to DateTimeNULL.</returns>
		public static bool operator >= (string s, DateTimeNULL dte)
		{
			if ((s > dte) || (s == dte))
				return true;
			else
				return false;
		}


		#endregion

		#region <=


		/// <summary>
		/// Overloads the Less Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DateTimeNULL value is less than or equal to DBNull.Value.</returns>
		public static bool operator <= (DateTimeNULL dte, DBNull Null)
		{
			if ((dte < Null) || (dte == Null))
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// Overloads the Less Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="Null">A DBNull value.</param>
		/// <returns>True if the DBNull.Value is less than or equal to DateTimeNULL.</returns>
		public static bool operator <= (DBNull Null, DateTimeNULL dte)
		{
			if ((Null < dte) || (Null == dte))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Less Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTimeNULL value is less than or equal to the DateTime value.</returns>
		public static bool operator <= (DateTimeNULL dte, DateTime dte2)
		{
			if ((dte < dte2) || (dte == dte2))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Less Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">A DateTime value.</param>
		/// <returns>True if the DateTime value is less than or equal to DateTimeNULL.</returns>
		public static bool operator <= (DateTime dte2, DateTimeNULL dte)
		{
			if ((dte2 < dte) || (dte2 == dte))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Less Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="dte2">Another DateTimeNULL value.</param>
		/// <returns>True if the DateTimeNULL value is less than or equal to the other DateTimeNULL value.</returns>
		public static bool operator <= (DateTimeNULL dte, DateTimeNULL dte2)
		{
			if ((dte < dte2) || (dte == dte2))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Less Than or Equal To Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the DateTimeNULL value is less than or equal to the string value.</returns>
		public static bool operator <= (DateTimeNULL dte, string s)
		{
			if ((dte < s) || (dte == s))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Overloads the Less Than or Equal Binary Comparison.
		/// </summary>
		/// <param name="dte">A DateTimeNULL value.</param>
		/// <param name="s">A string value.</param>
		/// <returns>True if the string value is less than or equal to DateTimeNULL.</returns>
		public static bool operator <= (string s, DateTimeNULL dte)
		{
			if ((s < dte) || (s == dte))
				return true;
			else
				return false;
		}


		#endregion


		#endregion

		#region Properties

		/// <summary>
		/// Gets a boolean flag that indicates that the current instance if the date is a Null Value.
		/// </summary>
		public bool IsNull
		{
			get
			{
				return _isNull;
			}
		}

        public DateTimeKind Kind
        {
            get
            {
                if (_isNull)
                    return DateTimeKind.Unspecified;
                else
                    return _date.Kind;
            }
        }

		public DateTimeNULL Date
		{
			get
			{
				if (_isNull)
					return DBNull.Value;
				else
					return _date.Date;
			}
		}

		public int Day
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Day;
			}
		}

		public int DayOfYear
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.DayOfYear;
			}
		}

		public int Hour
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Hour;
			}
		}

		public int Millesecond
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Millisecond;
			}
		}

		public int Minute
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Minute;
			}
		}

		public int Month
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Month;
			}
		}

		public int Second
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Second;
			}
		}

		public long Ticks
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Ticks;
			}
		}

		public int Year
		{
			get
			{
				if (_isNull)
					return -1;
				else
					return _date.Year;
			}
		}

		#endregion

		#region Object Implementation

		public override bool Equals(object obj)
		{
			if (obj is DBNull)
			{
				DBNull val = (DBNull)obj;
				if (this == val) 
					return true;
				else
					return false;
			}
			else if (obj is DateTime)
			{
				DateTime val = (DateTime)obj;
				if (this == val) 
					return true;
				else
					return false;
			}
			else if (obj is DateTimeNULL)
			{
				DateTimeNULL val = (DateTimeNULL)obj;
				if (this == val) 
					return true;
				else
					return false;
			}
			else if (obj is string)
			{
				string val = (string)obj;
				if (this == val) 
					return true;
				else
					return false;
			}
			return base.Equals (obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			if (_isNull)
				return Convert.ToString(DBNull.Value);
			else
				return Convert.ToString(_date);
		}

		public string ToShortDateString()
		{
			if (_isNull)
				return "";
			else
				return _date.ToShortDateString();
		}

		public DateTime DateTime
		{
			get
			{
				return _date;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Converts the date time into its actual value type.
		/// </summary>
		/// <returns></returns>
		public object ToObject()
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date;
		}


		public DateTimeNULL Add(TimeSpan value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.Add(value);
		}

		public DateTimeNULL AddDays(double value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddDays(value);
		}

		public DateTimeNULL AddHours(double value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddHours(value);
		}

		public DateTimeNULL AddMilliseconds(double value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddMilliseconds(value);
		}

		public DateTimeNULL AddMinutes(double value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddMinutes(value);
		}

		public DateTimeNULL AddMonths(int months)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddMonths(months);
		}

		public DateTimeNULL AddSeconds(double value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddSeconds(value);
		}

		public DateTimeNULL AddTicks(long value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddTicks(value);
		}

		public DateTimeNULL AddYears(int value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.AddYears(value);
		}

		public DateTimeNULL Subtract(TimeSpan value)
		{
			if (_isNull)
				return DBNull.Value;
			else
				return _date.Subtract(value);
		}

		public string ToString(string format, IFormatProvider provider)
		{		
			if (_isNull)
				return Convert.ToString(DBNull.Value);
			else
				return _date.ToString(format, provider);
		}

        public DateTimeNULL ToLocalTime()
        {
            if (_isNull)
                return DBNull.Value;
            else
                return _date.ToLocalTime();
        }

        public DateTimeNULL ToUniversalTime()
        {
            if (_isNull)
                return DBNull.Value;
            else
                return _date.ToUniversalTime();
        } 

		#endregion

		#region IFormattable Implementation

		public string ToString(IFormatProvider provider)
		{
			if (_isNull)
				return Convert.ToString(DBNull.Value);
			else
				return _date.ToString(provider);
		}

		#endregion

		#region IComparable Implementation

		public int CompareTo(object value)
		{
			if (value is DBNull)
			{
				DBNull val = (DBNull)value;
				if (this == val) return 0;
				if (this > val) return 1;
				if (this < val) return -1;
			}
			else if (value is DateTime)
			{
				DateTime val = (DateTime)value;
				if (this == val) return 0;
				if (this > val) return 1;
				if (this < val) return -1;
			}
			else if (value is DateTimeNULL)
			{
				DateTimeNULL val = (DateTimeNULL)value;
				if (this == val) return 0;
				if (this > val) return 1;
				if (this < val) return -1;
			}
			else if (value is string)
			{
				string val = (string)value;
				if (this == val) return 0;
				if (this > val) return 1;
				if (this < val) return -1;
			}
			if (!_isNull)
			{
				return _date.CompareTo(value);
			}

			throw new ArgumentException("object is not valid.");


		}

		#endregion

		#region Static Methods

		public static DateTimeNULL Parse (string s)
		{
			DateTimeNULL dte = new DateTimeNULL();
			if (s == null || s.Trim() == "")
				dte = DBNull.Value;
			else
				dte = DateTime.SpecifyKind(DateTime.Parse(s), DateTimeKind.Local);
			return dte;
		}

		public static DateTimeNULL Parse (string s, IFormatProvider provider)
		{
			DateTimeNULL dte = new DateTimeNULL();
			if (s == null || s.Trim() == "")
				dte = DBNull.Value;
			else
				dte = DateTime.SpecifyKind(DateTime.Parse(s, provider), DateTimeKind.Local);
			return dte;
		}

		public static DateTimeNULL Parse (string s, IFormatProvider provider, System.Globalization.DateTimeStyles styles)
		{
			DateTimeNULL dte = new DateTimeNULL();
			if (s == null || s.Trim() == "")
				dte = DBNull.Value;
			else
				dte = DateTime.SpecifyKind(DateTime.Parse(s, provider, styles), DateTimeKind.Local);
			return dte;
		}


		#endregion

		#region IConvertible Members

		public ulong ToUInt64(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToUInt64(_date,provider);
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToSByte(_date,provider);
		}

		public double ToDouble(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToDouble(_date,provider);
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			if (_isNull)
				return DateTime.MinValue.ToLocalTime();
			else
				return _date;
		}

		public float ToSingle(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToSingle(_date,provider);
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			if (_isNull)
				return false;
			else
				return Convert.ToBoolean(_date,provider);
		}

		public int ToInt32(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToInt32(_date,provider);
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToUInt16(_date,provider);
		}

		public short ToInt16(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToInt16(_date,provider);
		}

		public byte ToByte(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToByte(_date,provider);
		}

		public char ToChar(IFormatProvider provider)
		{
			return '\0';
		}

		public long ToInt64(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToInt64(_date,provider);
		}

		public System.TypeCode GetTypeCode()
		{
			return new System.TypeCode ();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToDecimal(_date,provider);
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(_date,conversionType,provider);
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			if (_isNull)
				return 0;
			else
				return Convert.ToUInt32(_date);
		}

		#endregion
	}
}

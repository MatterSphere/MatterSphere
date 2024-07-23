#region References
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	internal static class Occurance
	{
		#region Public Methods
		/// <summary>
		/// Returns the interval from the specific time to the next occurance of the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static TimeSpan Interval(TimeSpan occuranceTime)
		{
			return Interval(DateTime.Now, occuranceTime);
		}

		/// <summary>
		/// Returns the interval from the specific time to the next occurance of the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="fromTime">
		/// The from Time.
		/// </param>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static TimeSpan Interval(DateTime fromTime, TimeSpan occuranceTime)
		{
			return Interval(fromTime, occuranceTime, null);
		}

		/// <summary>
		/// Returns the interval from the specific time to the next occurance of the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <param name="occuranceDays">
		/// A list of the days of the week when the time may occur.
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static TimeSpan Interval(TimeSpan occuranceTime, IEnumerable<DayOfWeek> occuranceDays)
		{
			return Interval(DateTime.Now, occuranceTime, occuranceDays);
		}

		/// <summary>
		/// Returns the interval from the specific time to the next occurance of the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="fromTime">
		/// The from Time.
		/// </param>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <param name="occuranceDays">
		/// A list of the days of the week when the time may occur.
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static TimeSpan Interval(DateTime fromTime, TimeSpan occuranceTime, IEnumerable<DayOfWeek> occuranceDays)
		{
			return Next(fromTime, occuranceTime, occuranceDays).Subtract(fromTime);
		}

		/// <summary>
		/// Returns the date and time of the next occurance from the specific time to the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static DateTime Next(TimeSpan occuranceTime)
		{
			return Next(DateTime.Now, occuranceTime);
		}

		/// <summary>
		/// Returns the date and time of the next occurance from the specific time to the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="fromTime">
		/// The from Time.
		/// </param>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static DateTime Next(DateTime fromTime, TimeSpan occuranceTime)
		{
			return Next(fromTime, occuranceTime, null);
		}

		/// <summary>
		/// Returns the date and time of the next occurance from the specific time to the time on one the next matching day or the same day if no days are provided.
		/// </summary>
		/// <param name="fromTime">
		/// The from Time.
		/// </param>
		/// <param name="occuranceTime">
		/// The time of day for the occurence
		/// </param>
		/// <param name="occuranceDays">
		/// A list of the days of the week when the time may occur.
		/// </param>
		/// <returns>
		/// the timespan from now until the next occurance of the time on a given day.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The time string is invalid
		/// </exception>
		public static DateTime Next(DateTime fromTime, TimeSpan occuranceTime, IEnumerable<DayOfWeek> occuranceDays)
		{
			var relativeTime = new DateTime(
				fromTime.Year,
				fromTime.Month,
				fromTime.Day,
				occuranceTime.Hours,
				occuranceTime.Minutes,
				occuranceTime.Seconds,
				occuranceTime.Milliseconds);

			// The date will be todays date with the time value
			// If specific days are listed))
			if (occuranceDays != null)
			{
				// Advance the date to the nearest day in the collection
				// Example, today is day 2 and nearest day is day 5
				// Calc diff, then add days
				var diff = 0;

				// Get the next day this should occur
				var orderedDays = occuranceDays.OrderBy(k => k);

				foreach (var dayOfWeek in orderedDays)
				{
					// Could be today, a day later in the week, a day earlier in the week
					diff = dayOfWeek - relativeTime.DayOfWeek;

					if (relativeTime.DayOfWeek <= dayOfWeek)
					{
						break;
					}
				}

				// If the day was earlier in the week
				if (diff < 0)
				{
					// Advance it to the next week
					diff = diff + 7;
				}

				relativeTime = relativeTime.AddDays(diff);
			}

			// If the time is prior advance the day
			if (relativeTime < fromTime)
			{
				relativeTime = relativeTime.AddDays(1);
			}

			return relativeTime;
		}
		#endregion
	}
}

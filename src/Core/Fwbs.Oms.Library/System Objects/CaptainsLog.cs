using System;
using System.Data;

namespace FWBS.OMS.Logging
{
	/// <summary>
	/// A class that controls the flow to captains log entries within the client side offline system.
	/// Otherwise, all other logging is controlled by triggers and stored procedures on the database
	/// server itself.
	/// </summary>
	public class CaptainsLog
	{
		#region Fields

		/// <summary>
		/// A refrence to the session captains log table.
		/// </summary>
		private static DataTable _log = null;

		/// <summary>
		/// A refrence to the session captains log type table.
		/// </summary>
		private static DataTable _logType = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal CaptainsLog()
		{
		}

		/// <summary>
		/// Check the login state before any static methods are called.
		/// </summary>
		static CaptainsLog()
		{
			Session.CurrentSession.CheckLoggedIn();
			_log = Session.CurrentSession._regInfo.Tables[Session.Table_Log];
			_logType = Session.CurrentSession._regInfo.Tables[Session.Table_LogType];
		}

		#endregion

		/// <summary>
		/// Creates a new captains log entry within the cached captains log schema table.
		/// </summary>
		/// <param name="type">The type id of the log entry.</param>
		/// <param name="description">The brief description of the entry.</param>
		/// <param name="linkData">A link to another table.</param>
		/// <param name="extended">Any extended information needed.</param>
		/// <param name="cache">If true then the entry is added to the cached version. Otherwise it is added straight to the database.</param>
		public static void CreateEntry(short type, string description, object linkData, string extended, bool cache)
		{
			DataView vw = _logType.DefaultView;
			vw.RowFilter = "typeid = '" + type.ToString() + "'";

			if (vw.Count == 0 || (byte)vw[0]["typeseverity"] < Session.CurrentSession.LoggingActivitySeverity)
				return;

			if (cache)
			{
				DataRow row = _log.NewRow();
				row["logtypeid"] = type;
				row["logwhen"] = DateTime.Now;
				row["logusrID"] = Session.CurrentSession.CurrentUser.ID;
				row["logdesc"] = description;

				if (linkData is String)
				{
					row["logdataS"] = linkData;
					row["logdataN"] = DBNull.Value;
				}
				else
				{
					try
					{
						long val = Convert.ToInt64(linkData);
						row["logdataS"] = DBNull.Value;
						row["logdataN"] = val;
					}
					catch
					{
						row["logdataS"] = DBNull.Value;
						row["logdataN"] = DBNull.Value;
					}
				}

				if (extended.Trim() == String.Empty)
					row["logextended"] = DBNull.Value;
				else
					row["logextended"] = extended;

				_log.Rows.Add(row);
			}
			else
			{
				object linkN; object linkS;

				if (linkData is String)
				{
					linkS = linkData;
					linkN = DBNull.Value;
				}
				else
				{
					try
					{
						long val = Convert.ToInt64(linkData);
						linkS = DBNull.Value;
						linkN = val;
					}
					catch
					{
						linkS = DBNull.Value;
						linkN = DBNull.Value;
					}
				}

				IDataParameter[] paramList = new IDataParameter [6];
				paramList[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.SmallInt, 0, type);
				paramList[1] = Session.CurrentSession.Connection.AddParameter("User", SqlDbType.Int, 0, Session.CurrentSession.CurrentUser.ID);
				paramList[2] = Session.CurrentSession.Connection.AddParameter("Description", SqlDbType.NVarChar, 1200, description);
				paramList[3] = Session.CurrentSession.Connection.AddParameter("StringLink", SqlDbType.NVarChar, 100, linkS);
				paramList[4] = Session.CurrentSession.Connection.AddParameter("NumericLink", SqlDbType.BigInt, 0, linkN);
		
				if (extended.Trim() == String.Empty)
					paramList[5] = Session.CurrentSession.Connection.AddParameter("ExtendedInfo", SqlDbType.NText, 0, DBNull.Value);
				else
					paramList[5] = Session.CurrentSession.Connection.AddParameter("ExtendedInfo", SqlDbType.NText, 0, extended);

				Session.CurrentSession.Connection.ExecuteProcedure("sprCreateLogEntry", paramList);
			}
		}

		internal static void CreateEntry(LogType type, string description, object linkData, string extended, bool cache)
		{
			CreateEntry((short)type, description, linkData, extended, cache);
		}


		/// <summary>
		/// Logs a login event.
		/// </summary>
		internal static void CreateLoginEntry()
		{
			CreateEntry(LogType.Login, "Logging In...", Session.CurrentSession.CurrentUser.ID, "", false);
		}

		/// <summary>
		/// Logs a logoff event.
		/// </summary>
		internal static void CreateLogoffEntry()
		{
			CreateEntry(LogType.Logoff, "Logging Off...", Session.CurrentSession.CurrentUser.ID, "", false);
		}

		/// <summary>
		/// Creates Client Log Entry.
		/// </summary>
		internal static void CreateClientEntry(ClientLogType lt,string desc,string extended)
		{
			CreateEntry((short)lt, desc, Session.CurrentSession.CurrentUser.ID, extended, false);
		}

		/// <summary>
		/// Flushes all of the cached captains log entries into the database.
		/// </summary>
		public static void Flush()
		{
			if (_log.GetChanges() != null)
			{ 
				IDataParameter[] paramList = new IDataParameter [7];
				paramList[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.SmallInt, 0, 0);
				paramList[0].SourceColumn = "logtypeid";
				paramList[1] = Session.CurrentSession.Connection.AddParameter("User", SqlDbType.Int, 0, 0);
				paramList[1].SourceColumn = "logusrid";
				paramList[2] = Session.CurrentSession.Connection.AddParameter("DateTime", SqlDbType.DateTime, 1200, DBNull.Value);
				paramList[2].SourceColumn = "logwhen";
				paramList[3] = Session.CurrentSession.Connection.AddParameter("Description", SqlDbType.NVarChar, 1200, "");
				paramList[3].SourceColumn = "logdesc";
				paramList[4] = Session.CurrentSession.Connection.AddParameter("StringLink", SqlDbType.NVarChar, 100, "");
				paramList[4].SourceColumn = "logdataS";
				paramList[5] = Session.CurrentSession.Connection.AddParameter("NumericLink", SqlDbType.BigInt, 0, 0);
				paramList[5].SourceColumn = "logdataN";
				paramList[6] = Session.CurrentSession.Connection.AddParameter("ExtendedInfo", SqlDbType.NText, 0, "");
				paramList[6].SourceColumn = "logextended";
				Session.CurrentSession.Connection.Update(_log, "sprCreateLogEntry", null, null, paramList);
				_log.Clear();
				_log.AcceptChanges();
			}
		}

	}

	/// <summary>
	/// All the log type codes / id's.
	/// </summary>
	public enum LogType
	{
		Login = 1,
		Logoff = 2,
		ClientFirmContact = 3
	}
}

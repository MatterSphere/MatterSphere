using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// Summary description for ReportingServer.
	/// </summary>
	public class ReportingServer
	{
		private ReportingServer()
		{
		}

		public ReportingServer(string licenseCode)
		{
			if (licenseCode != "FWBS Limited 2005")
				throw new Exception("You are not licensed to use this Library");
		}

		public FWBS.OMS.Data.Connection Connection
		{
			get
			{
				return FWBS.OMS.Session.CurrentSession.Connection;
			}
		}

		public static DataTable ListAvailableReports()
		{
			IDataParameter[] paramlist = new IDataParameter[0];
			DataTable table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from rsReports","REPORTS",false,paramlist);
			DataView view = new DataView(table,"","",DataViewRowState.OriginalRows);
			foreach (DataRowView rw in view)
			{
				string reportroles = Convert.ToString(rw["repUserGroup"]);
				if (Session.CurrentSession.CurrentUser.IsInRoles(reportroles.Split(";".ToCharArray())) == false)
				{
					rw.Delete();
				}
			}
			return table;
		}

	}
}

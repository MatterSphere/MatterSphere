using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// 22000 Summary description for Application.
	/// </summary>
    [Obsolete("FWBS.OMS.RegisteredApplication is now obsolete, please use FWBS.OMS.Application.RegisteredApplication")]
	public class RegisteredApplication
	{
		#region Fields
		
		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _application = null;
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		private const string Sql = "select * from dbapplication";
		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		private const string Table = "APPLICATION";


		#endregion

		#region Constructors

		/// <summary>
		/// Initaites this class and fetches a single application.
		/// </summary>
		/// <param name="id">Simple Application identifier</param>
		internal RegisteredApplication (long id)
		{
			//Make sure that the parameters list is cleared after use.	
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("APPID", System.Data.SqlDbType.BigInt, 0, id);
			_application = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + "  WHERE APPID = @APPID", Table, paramlist);
			
			if ((_application== null) || (_application.Rows.Count == 0)) 
				throw new OMSException(HelpIndexes.ApplicationNotFound, id.ToString());

		}

		/// <summary>
		/// Initaites this class and fetches a single store item from the database.
		/// </summary>
		/// <param name="guid">Application global unique identifier.</param>
		internal RegisteredApplication (Guid guid)
		{
			//Make sure that the parameters list is cleared after use.	
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("APPGUID", System.Data.SqlDbType.UniqueIdentifier, 0, guid);
			_application = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + "  WHERE APPGUID = @APPGUID", Table, paramlist);
			
			if ((_application== null) || (_application.Rows.Count == 0)) 
				throw new OMSException(HelpIndexes.ApplicationNotFound, guid.ToString());

		}

		public override string ToString()
		{
			return Convert.ToString(GetExtraInfo("appName"));
		}

		
		#endregion

		#region Properties

		
		public long ID
		{
			get
			{
				return Convert.ToInt16(GetExtraInfo("appID"));
			}
		}


		public string AppCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appCode"));
			}
		}

		public string AppVersion
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appVersion"));
			}
		}

		public string AppName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appName"));
			}
		}

		public string AppPath
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appPath"));
			}
		}

		public string AppProgID
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appProgID"));
			}
		}

		public string AppPrintCmd
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appPrintCmd"));
			}
		}

		public string AppOpenCmd
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appOpenCmd"));
			}
		}

		public System.Drawing.Icon AppIcon
		{
			get
			{
				return GetExtraInfo("appIcon") as System.Drawing.Icon;
			}
		}

		public bool AppAutomated
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("appAutomated"));
			}
		}
		
		public string AppTypeString
		{
			get
			{
				return Convert.ToString(GetExtraInfo("appType"));
			}
		}

		public Type AppType
		{
			get
			{
				if (Convert.ToString(GetExtraInfo("AppType")) == "")
					return null;
				// Try and create and instantiate the object through the type name
				try
				{
					Type objtype;
                    objtype = Session.CurrentSession.TypeManager.Load(Convert.ToString(GetExtraInfo("appType")));
					return objtype;				
				}
				catch (Exception ex)
				{
					throw new OMSException(ex,HelpIndexes.ApplicationNotCreateable,new string [] {GetExtraInfo("AppType").ToString()});
				}
			}
		}

		/// <summary>
		/// Gets the default blank precedent for the application type.
		/// </summary>
		public Precedent BlankPrecedent
		{
			get
			{
				object id = GetExtraInfo("appBlankPrecedent");
				if (id == DBNull.Value)
					return Precedent.GetPrecedent("DEFAULT", Convert.ToString(GetExtraInfo("appBlankPrecedentType")), "", "", "", Session.CurrentSession.DefaultCulture, ""); 
				else
					return Precedent.GetPrecedent(Convert.ToInt64(id));
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates an instance of the OMSApp.
		/// </summary>
		/// <returns></returns>
		public Interfaces.IOMSApp CreateOMSApp()
		{
			return AppType.InvokeMember("",System.Reflection.BindingFlags.CreateInstance,null,null,null) as Interfaces.IOMSApp;
		}

		#endregion

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public virtual void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }
			_application.Rows[0][fieldName] = val;
		}
		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{
			object val = _application.Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}
		/// <summary>
		/// Returns the specified fields data.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public virtual Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _application.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("22001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}
		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public virtual DataSet GetDataset()
		{
			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}
		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public virtual DataTable GetDataTable()
		{
			return _application.Copy();
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a registered application object by its id.
		/// </summary>
		/// <param name="id">Application identifier.</param>
		/// <returns>Registered application.</returns>
		public static RegisteredApplication GetApplication(int id)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new RegisteredApplication(id);
		}

		/// <summary>
		/// Gets a registered application object by its GUID.
		/// </summary>
		/// <param name="guid">Application unique identifier.</param>
		/// <returns>Registered application.</returns>
		public static RegisteredApplication GetApplication(Guid guid)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new RegisteredApplication(guid);
		}

		#endregion
	}



}
